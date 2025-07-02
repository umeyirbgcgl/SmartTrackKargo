namespace SmartTrackKargo.Models
{
    public class Parcel
    {
        public int Id { get; set; }
        public string TrackingCode { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Receiver { get; set; } = string.Empty;
        public string Status { get; set; } = "Beklemede";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsDelivered { get; set; }
    }
}
