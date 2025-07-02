using System;
using System.Threading.Tasks;
using SmartTrackKargo.Models;

namespace SmartTrackKargo.Services
{
    public class PredictionService
    {
        public bool TahminEt(string sicaklik, string nem, string havaDurumu)
        {
            // Basit kural tabanlı tahmin sistemi
            if (string.IsNullOrEmpty(havaDurumu))
            {
                return true; // Hava durumu bilgisi yoksa varsayılan olarak teslimat yapılabilir
            }

            var hava = havaDurumu.ToLower();
            
            // Kötü hava koşullarında teslimat yapılmaz
            if (hava.Contains("kar") || hava.Contains("yagmur") || hava.Contains("firtina"))
            {
                return false;
            }

            // String değerleri sayısal değerlere dönüştür
            if (!float.TryParse(sicaklik.Replace("°C", ""), out float sicaklikDeger))
            {
                return true; // Dönüşüm başarısız olursa varsayılan olarak true döndür
            }

            if (!float.TryParse(nem.Replace("%", ""), out float nemDeger))
            {
                return true; // Dönüşüm başarısız olursa varsayılan olarak true döndür
            }

            // Aşırı sıcaklıklarda teslimat yapılmaz
            if (sicaklikDeger < 0 || sicaklikDeger > 35)
            {
                return false;
            }

            // Yüksek nemde teslimat yapılmaz
            if (nemDeger > 85)
            {
                return false;
            }

            // Diğer durumlarda teslimat yapılabilir
            return true;
        }

        public async Task<bool> GetDeliveryPrediction(WeatherPrediction weather)
        {
            // Hava durumu verilerine göre teslimat tahmini yap
            double sicaklik = double.Parse(weather.Sicaklik);
            int nem = int.Parse(weather.Nem);

            // Teslimat için uygun koşulları kontrol et
            bool isTemperatureSafe = sicaklik >= 0 && sicaklik <= 35;
            bool isHumiditySafe = nem >= 30 && nem <= 80;
            bool isWeatherSafe = !weather.HavaDurumu.Contains("Fırtına") && 
                               !weather.HavaDurumu.Contains("Kar") &&
                               !weather.HavaDurumu.Contains("Şiddetli");

            return isTemperatureSafe && isHumiditySafe && isWeatherSafe;
        }
    }
}
