namespace SmartTrackKargo.Models
{
    public class WeatherPrediction
    {
        public int Id { get; set; }
        public string Sicaklik { get; set; } = string.Empty;
        public string Nem { get; set; } = string.Empty;
        public string HavaDurumu { get; set; } = string.Empty;
        public string Tarih { get; set; } = string.Empty;
        public bool IsDeliverySafe { get; set; }
        public string WeatherCondition { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public int Humidity { get; set; }
    }
}
