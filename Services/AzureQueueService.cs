using Azure.Storage.Queues;
using System.Text;
using System.Text.Json;
using TicketHubApi.Models;

namespace TicketHubApi.Services
{
    public class AzureQueueService
    {
        private readonly QueueClient _queueClient;

        public AzureQueueService(IConfiguration configuration)
        {
            string connectionString = configuration["AzureQueueConnection"];
            _queueClient = new QueueClient(connectionString, "tickethub");
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(Ticket ticket)
        {
            string json = JsonSerializer.Serialize(ticket);
            string base64Message = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            await _queueClient.SendMessageAsync(base64Message);
        }
    }
}
