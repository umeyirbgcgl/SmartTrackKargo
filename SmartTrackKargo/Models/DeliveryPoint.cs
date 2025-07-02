using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrackKargo.Models
{
    [Table("DeliveryPoints")]
    public class DeliveryPoint
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        [StringLength(200)]
        public string Info { get; set; } = string.Empty;

        public bool IsDelivered { get; set; }
    }
}
