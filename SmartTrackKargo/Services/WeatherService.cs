using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using SmartTrackKargo.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SmartTrackKargo.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(IConfiguration config, ILogger<WeatherService> logger)
        {
            _httpClient = new HttpClient();
            _apiKey = config.GetSection("OpenWeatherMap:ApiKey").Value ?? 
                throw new ArgumentNullException("OpenWeatherMap:ApiKey yapılandırması bulunamadı");
            _baseUrl = config.GetSection("OpenWeatherMap:BaseUrl").Value ?? 
                "https://api.openweathermap.org/data/2.5/weather";
            _logger = logger;
        }

        public async Task<WeatherPrediction> GetWeatherAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"{_baseUrl}?lat={latitude}&lon={longitude}&appid={_apiKey}&units=metric&lang=tr";
                
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<OpenWeatherResponse>(content);

                if (weatherData?.Main == null || weatherData.Weather == null || weatherData.Weather.Length == 0)
                {
                    _logger.LogWarning("API'den eksik veri alındı");
                    return GenerateTestWeatherData();
                }

                return new WeatherPrediction
                {
                    Sicaklik = weatherData.Main.Temp.ToString("F1"),
                    Nem = weatherData.Main.Humidity.ToString(),
                    HavaDurumu = MapWeatherDescription(weatherData.Weather[0].Main),
                    Tarih = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Hava durumu API'sine erişim hatası");
                return GenerateTestWeatherData();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Hava durumu verisi ayrıştırma hatası");
                return GenerateTestWeatherData();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Beklenmeyen hava durumu hatası");
                return GenerateTestWeatherData();
            }
        }

        private WeatherPrediction GenerateTestWeatherData()
        {
            var random = new Random();
            string[] havaDurumlari = { "Gunesli", "Parcali Bulutlu", "Yagmurlu", "Kapali" };
            
            return new WeatherPrediction
            {
                Sicaklik = Math.Round(random.NextDouble() * (35 - 15) + 15, 1).ToString("F1"),
                Nem = random.Next(40, 90).ToString(),
                HavaDurumu = havaDurumlari[random.Next(havaDurumlari.Length)],
                Tarih = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
            };
        }

        private string MapWeatherDescription(string englishDescription)
        {
            return englishDescription?.ToLower() switch
            {
                "clear" => "Gunesli",
                "clouds" => "Parcali Bulutlu",
                "rain" => "Yagmurlu",
                "drizzle" => "Hafif Yagmurlu",
                "thunderstorm" => "Firtinali",
                "snow" => "Karli",
                "mist" => "Sisli",
                "fog" => "Sisli",
                _ => "Kapali"
            };
        }

        private class OpenWeatherResponse
        {
            public MainInfo Main { get; set; } = new();
            public WeatherInfo[] Weather { get; set; } = Array.Empty<WeatherInfo>();
        }

        private class MainInfo
        {
            public double Temp { get; set; }
            public int Humidity { get; set; }
        }

        private class WeatherInfo
        {
            public string Main { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }
    }
} 