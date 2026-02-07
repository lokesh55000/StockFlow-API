using Microsoft.AspNetCore.Mvc;

namespace StockFlow.API.Controllers
{
    [ApiController]
    [Route("api/health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok(new
            {
                status = "Healthy",
                service = "StockFlow API",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
