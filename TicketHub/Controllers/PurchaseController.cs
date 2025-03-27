using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Queues;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;

namespace TicketHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly ILogger<PurchaseController> _logger;
        private readonly IConfiguration _configuration;

        // Constructor
        public PurchaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Purchase purchase) // FromBody binds the JSON to the Purchase object
        {
            if (ModelState.IsValid == false)
            {
               return BadRequest(ModelState);
            }

            // If everything valid, send to Azure queue
            string queueName = "purchases";

            // Get connection string from secrets.json
            string? connectionString = _configuration["AzureStorageConnectionString"];

            if (string.IsNullOrEmpty(connectionString))
            {
                return BadRequest("Connection string error.");
            }

            QueueClient queueClient = new QueueClient(connectionString, queueName);

            // serialize an object to json
            string message = JsonSerializer.Serialize(purchase);

            // send string message to queue
            await queueClient.SendMessageAsync(message);

            return Ok("Message sent to Azure queue successfully.");
        }
    }
}
