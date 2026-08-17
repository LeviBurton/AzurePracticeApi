using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzurePractice.Functions;

public class PracticeHttpFunction
{
    private readonly ILogger<PracticeHttpFunction> _logger;

    public PracticeHttpFunction(ILogger<PracticeHttpFunction> logger)
    {
        _logger = logger;
    }

    [Function("PracticeHttpFunction")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
