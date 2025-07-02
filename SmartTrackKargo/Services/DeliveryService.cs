using System;
using System.Collections.Generic;
using System.Linq;
using SmartTrackKargo.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace SmartTrackKargo.Services
{
    public class DeliveryService
    {
        private static List<DeliveryPoint> _deliveryPoints;

        public DeliveryService()
        {
            if (_deliveryPoints == null)
            {
                _deliveryPoints = new List<DeliveryPoint>
                {
                    new DeliveryPoint { Id = 1, Info = "Gebze Merkez", Latitude = 40.7929, Longitude = 29.4321, IsDelivered = false },
                    new DeliveryPoint { Id = 2, Info = "Gebze Teknik Üniversitesi", Latitude = 40.7889, Longitude = 29.4399, IsDelivered = false },
                    new DeliveryPoint { Id = 3, Info = "Gebze Center AVM", Latitude = 40.7902, Longitude = 29.4308, IsDelivered = false },
                    new DeliveryPoint { Id = 4, Info = "GOSB Teknopark", Latitude = 40.7950, Longitude = 29.4276, IsDelivered = false },
                    new DeliveryPoint { Id = 5, Info = "Eskihisar Kalesi", Latitude = 40.7863, Longitude = 29.4183, IsDelivered = false },
                    new DeliveryPoint { Id = 6, Info = "TÜBİTAK MAM", Latitude = 40.7998, Longitude = 29.4557, IsDelivered = false }
                };
            }
        }

        public List<DeliveryPoint> GetDeliveryPoints()
        {
            return _deliveryPoints;
        }

        public bool MarkAsDelivered(int id)
        {
            var point = _deliveryPoints.FirstOrDefault(p => p.Id == id);
            if (point != null)
            {
                point.IsDelivered = true;
                return true;
            }
            return false;
        }

        public Task<List<DeliveryPoint>> GetAllDeliveryPointsAsync()
        {
            return Task.FromResult(_deliveryPoints);
        }

        public Task<bool> MarkAsDeliveredAsync(int id)
        {
            var point = _deliveryPoints.Find(p => p.Id == id);
            if (point == null) return Task.FromResult(false);

            point.IsDelivered = true;
            return Task.FromResult(true);
        }

        public Task<DeliveryPoint> AddDeliveryPointAsync(DeliveryPoint point)
        {
            point.Id = _deliveryPoints.Count + 1;
            _deliveryPoints.Add(point);
            return Task.FromResult(point);
        }

        public Task<bool> UpdateDeliveryPointAsync(DeliveryPoint point)
        {
            var existing = _deliveryPoints.Find(p => p.Id == point.Id);
            if (existing == null) return Task.FromResult(false);

            existing.Latitude = point.Latitude;
            existing.Longitude = point.Longitude;
            existing.Info = point.Info;
            existing.IsDelivered = point.IsDelivered;

            return Task.FromResult(true);
        }
    }
}
