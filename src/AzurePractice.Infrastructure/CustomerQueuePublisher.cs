using System.Text.Json;
using Azure.Storage.Queues;
using AzurePractice.Application;

namespace AzurePractice.Infrastructure;

public class CustomerQueuePublisher : ICustomerQueuePublisher
{
    private readonly QueueClient _queueClient;

    public CustomerQueuePublisher(QueueClient queueClient)
    {
        _queueClient = queueClient;
    }

    public async Task EnqueueAsync(
        int customerId,
        string action,
        CancellationToken cancellationToken = default)
    {
        var message = new
        {
            CustomerId = customerId,
            Action = action
        };

        var json = JsonSerializer.Serialize(message);

        await _queueClient.SendMessageAsync(
            json,
            cancellationToken);
    }
}