using Microsoft.EntityFrameworkCore;
using SmartTrackKargo.Models;

namespace SmartTrackKargo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DeliveryPoint> DeliveryPoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial delivery points for Gebze region
            modelBuilder.Entity<DeliveryPoint>().HasData(
                new DeliveryPoint { Id = 1, Info = "Gebze Organize Sanayi", Latitude = 40.790335, Longitude = 29.521482, IsDelivered = false },
                new DeliveryPoint { Id = 2, Info = "Gebze Teknik Üniversitesi", Latitude = 40.798214, Longitude = 29.451827, IsDelivered = false },
                new DeliveryPoint { Id = 3, Info = "Gebze Center", Latitude = 40.802358, Longitude = 29.439603, IsDelivered = false },
                new DeliveryPoint { Id = 4, Info = "Darıca Sahil", Latitude = 40.777401, Longitude = 29.371803, IsDelivered = false },
                new DeliveryPoint { Id = 5, Info = "Eskihisar Kalesi", Latitude = 40.764957, Longitude = 29.376744, IsDelivered = false },
                new DeliveryPoint { Id = 6, Info = "Çayırova Metro", Latitude = 40.824772, Longitude = 29.374042, IsDelivered = false },
                new DeliveryPoint { Id = 7, Info = "Tübitak MAM", Latitude = 40.786795, Longitude = 29.459670, IsDelivered = false },
                new DeliveryPoint { Id = 8, Info = "Mutlukent", Latitude = 40.789817, Longitude = 29.487813, IsDelivered = false },
                new DeliveryPoint { Id = 9, Info = "Pelikan AVM", Latitude = 40.786097, Longitude = 29.419987, IsDelivered = false },
                new DeliveryPoint { Id = 10, Info = "Körfez Tren İstasyonu", Latitude = 40.775202, Longitude = 29.398914, IsDelivered = false }
            );
        }
    }
} 