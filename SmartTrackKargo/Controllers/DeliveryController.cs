using Microsoft.AspNetCore.Mvc;
using SmartTrackKargo.Models;
using SmartTrackKargo.Services;
using System.Threading.Tasks;

namespace SmartTrackKargo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly DeliveryService _deliveryService;

        public DeliveryController(DeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var points = await _deliveryService.GetAllDeliveryPointsAsync();
            return Ok(points);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DeliveryPoint point)
        {
            var result = await _deliveryService.AddDeliveryPointAsync(point);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DeliveryPoint point)
        {
            if (id != point.Id)
                return BadRequest();

            var success = await _deliveryService.UpdateDeliveryPointAsync(point);
            if (!success)
                return NotFound();

            return Ok();
        }

        [HttpPost("{id}/mark-delivered")]
        public async Task<IActionResult> MarkAsDelivered(int id)
        {
            var success = await _deliveryService.MarkAsDeliveredAsync(id);
            if (!success)
                return NotFound();

            return Ok();
        }
    }
}
