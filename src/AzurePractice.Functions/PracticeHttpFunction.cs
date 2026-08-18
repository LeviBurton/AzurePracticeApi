using AzurePractice.Application;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AzurePractice.Functions;

public class PracticeHttpFunction
{
    private readonly ILogger<PracticeHttpFunction> _logger;
    private readonly ICustomerService _customerService;

    public PracticeHttpFunction(
        ILogger<PracticeHttpFunction> logger,
        ICustomerService customerService)
    {
        _logger = logger;
        _customerService = customerService;
    }

    [Function("PracticeHttpFunction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        var customers = await _customerService.GetAllAsync();

        return new OkObjectResult(new
        {
            Message = "Azure Function reached Azure SQL successfully.",
            CustomerCount = customers.Count
        });
    }
}