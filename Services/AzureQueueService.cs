using Azure.Storage.Queues;
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
            string message = JsonSerializer.Serialize(ticket);
            await _queueClient.SendMessageAsync(message);
        }
    }
}
