using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TicketHubApi.Models;
using TicketHubApi.Services;

namespace TicketHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ILogger<TicketsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AzureQueueService _queueService;

        public TicketsController(ILogger<TicketsController> logger, IConfiguration configuration, AzureQueueService queueService)
        {
            _logger = logger;
            _configuration = configuration;
            _queueService = queueService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello from the TicketsController - GET");
        }

        [HttpPost]
        public async Task<IActionResult> PurchaseTicket([FromBody] Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Processing ticket purchase for Concert ID: {ConcertId}", ticket.ConcertId);

            await _queueService.SendMessageAsync(ticket);

            return Ok(new { Message = "Ticket purchase request received and queued." });
        }
    }
}
