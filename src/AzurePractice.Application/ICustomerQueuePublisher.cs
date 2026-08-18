namespace AzurePractice.Application;

public interface ICustomerQueuePublisher
{
    Task EnqueueAsync(
        int customerId,
        string action,
        CancellationToken cancellationToken = default);
}