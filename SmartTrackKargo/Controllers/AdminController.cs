using Microsoft.AspNetCore.Mvc;
using SmartTrackKargo.Models;
using SmartTrackKargo.Services;
using System.Text.Json;
using System.Linq;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SmartTrackKargo.Controllers
{
    public class AdminController : Controller
    {
        private static List<DeliveryPoint> _deliveryPoints;
        private readonly AIAgent _aiAgent;
        private readonly WeatherService _weatherService;
        private readonly PredictionService _predictionService;
        private readonly DeliveryService _deliveryService;

        public AdminController(
            WeatherService weatherService, 
            PredictionService predictionService,
            DeliveryService deliveryService,
            AIAgent aiAgent,
            IConfiguration configuration)
        {
            _weatherService = weatherService;
            _predictionService = predictionService;
            _deliveryService = deliveryService;
            _aiAgent = aiAgent;
            _deliveryPoints = new List<DeliveryPoint>();
        }

        public IActionResult Index()
        {
            var points = _deliveryService.GetDeliveryPoints();
            return View(points);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsDelivered(int id)
        {
            var success = _deliveryService.MarkAsDelivered(id);
            if (success)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> AITahminKutucugu()
        {
            try
            {
                var weather = await _weatherService.GetWeatherAsync(41.0082, 28.9784); // İstanbul koordinatları
                var isDeliverySafe = await _predictionService.GetDeliveryPrediction(weather);

                return Json(new
                {
                    success = true,
                    sicaklik = weather.Sicaklik + "°C",
                    nem = weather.Nem + "%",
                    hava = weather.HavaDurumu,
                    teslimOlur = isDeliverySafe
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    error = "Hava durumu tahmini alınamadı: " + ex.Message
                });
            }
        }

        [HttpGet]
        public IActionResult GetDeliveryPoints()
        {
            var points = _deliveryService.GetDeliveryPoints();
            return Json(points);
        }
    }
}
