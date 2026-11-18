using FlowOps.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace FlowOps.API.Controllers
{
    [ApiController]
    [Route("api/database")]
    public class DatabaseController : ControllerBase
    {
        private readonly FlowOpsDbContext _context;
        private readonly ILogger<DatabaseController> _logger;

        public DatabaseController(
            FlowOpsDbContext context, 
            ILogger<DatabaseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("health")]
        public async Task<IActionResult> CheckHealth()
        {
            try
            {
                bool canConnect =
                    await _context.Database.CanConnectAsync();

                if (canConnect)
                {
                    return Ok(new
                    {
                        status = "Healthy",
                        database = "PostgreSQL"
                    });
                }

                _logger.LogWarning(
                    "PostgreSQL health check returned false.");

                return StatusCode(503, new
                {
                    status = "Unhealthy",
                    database = "PostgreSQL"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "PostgreSQL health check failed.");

                return StatusCode(503, new
                {
                    status = "Unhealthy",
                    database = "PostgreSQL"
                });
            }
        }
    }
}
