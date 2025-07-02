using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace SmartTrackKargo.Services
{
    public class AIAgent
    {
        private readonly HttpClient _httpClient;
        private readonly string _weatherApiKey;
        private const string GEBZE_LAT = "40.7929";
        private const string GEBZE_LON = "29.4321";

        public AIAgent(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _weatherApiKey = configuration["WeatherApi:ApiKey"] ?? "default_key";
        }

        public async Task<WeatherPrediction> GetWeatherPrediction()
        {
            try
            {
                // OpenWeatherMap API'den hava durumu verilerini al
                var response = await _httpClient.GetStringAsync(
                    $"http://api.openweathermap.org/data/2.5/weather?lat={GEBZE_LAT}&lon={GEBZE_LON}&appid={_weatherApiKey}&units=metric"
                );

                var weatherData = JsonSerializer.Deserialize<WeatherApiResponse>(response);

                var prediction = new WeatherPrediction
                {
                    Temperature = Math.Round(weatherData.Main.Temp),
                    Humidity = weatherData.Main.Humidity,
                    WeatherCondition = weatherData.Weather[0].Main,
                    IsDeliverySafe = AnalyzeWeatherConditions(weatherData),
                    HavaDurumu = weatherData.Weather[0].Description,
                    Nem = $"%{weatherData.Main.Humidity}",
                    Sicaklik = $"{Math.Round(weatherData.Main.Temp)}°C",
                    Tarih = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                };

                return prediction;
            }
            catch (Exception ex)
            {
                // API çalışmazsa varsayılan değerler döndür
                return new WeatherPrediction
                {
                    Temperature = 20,
                    Humidity = 65,
                    WeatherCondition = "Clear",
                    IsDeliverySafe = true,
                    HavaDurumu = "Açık",
                    Nem = "%65",
                    Sicaklik = "20°C",
                    Tarih = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                };
            }
        }

        private bool AnalyzeWeatherConditions(WeatherApiResponse weather)
        {
            // Hava durumu analizi
            if (weather.Main.Temp < 0 || weather.Main.Temp > 35)
                return false; // Aşırı sıcak veya soğuk

            if (weather.Main.Humidity > 90)
                return false; // Çok nemli

            // Kötü hava koşulları kontrolü
            var badConditions = new[] { "Thunderstorm", "Snow", "Rain" };
            if (Array.Exists(badConditions, condition => 
                weather.Weather[0].Main.Equals(condition, StringComparison.OrdinalIgnoreCase)))
                return false;

            return true;
        }
    }

    public class WeatherPrediction
    {
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public string WeatherCondition { get; set; } = string.Empty;
        public bool IsDeliverySafe { get; set; }
        public string HavaDurumu { get; set; } = string.Empty;
        public string Nem { get; set; } = string.Empty;
        public string Sicaklik { get; set; } = string.Empty;
        public string Tarih { get; set; } = string.Empty;
    }

    // OpenWeatherMap API response modelleri
    public class WeatherApiResponse
    {
        public MainInfo Main { get; set; }
        public WeatherInfo[] Weather { get; set; }
    }

    public class MainInfo
    {
        public double Temp { get; set; }
        public int Humidity { get; set; }
    }

    public class WeatherInfo
    {
        public string Main { get; set; }
        public string Description { get; set; }
    }
} 