using Microsoft.AspNetCore.Mvc;
using SmartTrackKargo.Services;
using System.Threading.Tasks;

namespace SmartTrackKargo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionController : ControllerBase
    {
        private readonly WeatherService _weatherService;
        private readonly PredictionService _predictionService;

        public PredictionController(WeatherService weatherService, PredictionService predictionService)
        {
            _weatherService = weatherService;
            _predictionService = predictionService;
        }

        [HttpGet("weather")]
        public async Task<IActionResult> GetWeather()
        {
            try
            {
                var weather = await _weatherService.GetWeatherAsync(40.7929, 29.4321);
                if (weather == null)
                {
                    return NotFound("Hava durumu verisi bulunamadı.");
                }

                return Ok(weather);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hava durumu verisi alınırken bir hata oluştu.");
            }
        }

        [HttpGet("delivery-prediction")]
        public async Task<IActionResult> GetDeliveryPrediction()
        {
            try
            {
                var weather = await _weatherService.GetWeatherAsync(40.7929, 29.4321);
                if (weather == null || string.IsNullOrEmpty(weather.Sicaklik) || 
                    string.IsNullOrEmpty(weather.Nem) || string.IsNullOrEmpty(weather.HavaDurumu))
                {
                    return NotFound("Hava durumu verisi eksik veya bulunamadı.");
                }

                var prediction = _predictionService.TahminEt(weather.Sicaklik, weather.Nem, weather.HavaDurumu);
                return Ok(new { prediction });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Tahmin yapılırken bir hata oluştu.");
            }
        }
    }
}
