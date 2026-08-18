namespace AzurePractice.Functions.Messages;

public class CustomerQueueMessage
{
    public int CustomerId { get; set; }

    public string Action { get; set; } = string.Empty;
}