using AzurePractice.Application;
using AzurePractice.Functions.Messages;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzurePractice.Functions;

public class CustomerQueueFunction
{
    private readonly ILogger<CustomerQueueFunction> _logger;
    private readonly ICustomerService _customerService;

    public CustomerQueueFunction(
        ILogger<CustomerQueueFunction> logger,
        ICustomerService customerService)
    {
        _logger = logger;
        _customerService = customerService;
    }

   [Function("CustomerQueueFunction")]
    public async Task Run(
        [QueueTrigger("customer-work-items")]
        CustomerQueueMessage message)
    {
        _logger.LogInformation(
            "Processing queue message. CustomerId: {CustomerId}, Action: {Action}",
            message.CustomerId,
            message.Action);

        var customer =
            await _customerService.GetByIdAsync(message.CustomerId);

        if (customer is null)
        {
            _logger.LogWarning(
                "Customer {CustomerId} was not found.",
                message.CustomerId);

            return;
        }

        _logger.LogInformation(
            "Processed customer {CustomerId}: {CustomerName}",
            customer.Id,
            customer.Name);
    }
}