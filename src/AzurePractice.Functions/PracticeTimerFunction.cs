using AzurePractice.Application;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzurePractice.Functions;

public class PracticeTimerFunction
{
    private readonly ILogger<PracticeTimerFunction> _logger;
    private readonly ICustomerService _customerService;

    public PracticeTimerFunction(
        ILogger<PracticeTimerFunction> logger,
        ICustomerService customerService)
    {
        _logger = logger;
        _customerService = customerService;
    }

    [Function("PracticeTimerFunction")]
    public async Task Run(
        [TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
    {
        var customers = await _customerService.GetAllAsync();

        _logger.LogInformation(
            "Timer executed. Customer count: {customerCount}",
            customers.Count);
    }
}