
using Microsoft.AspNetCore.Mvc;

namespace FlowOps.API.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult GetHealth()
    {
        return Ok(new
        {
            Status = "Healthy",
            Service = "FlowOps.API",
            Version = "1.0.0",
            Timestamp = DateTime.UtcNow
        });
    }
}
