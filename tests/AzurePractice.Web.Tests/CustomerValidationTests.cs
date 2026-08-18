using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AzurePractice.Web.Tests;

public class CustomerValidationTests
{
    [Fact]
    public async Task CreateCustomer_WithInvalidInput_ReturnsBadRequest()
    {
        await using var factory =
            new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    // The application requires a connection string at startup.
                    // This test never reaches the database, so a dummy value is enough.
                    builder.UseSetting(
                        "ConnectionStrings:DefaultConnection",
                        "Server=localhost;Database=TestDb;User Id=test;Password=test;");
                        
                    builder.UseSetting(
                            "ConnectionStrings:DefaultConnection",
                            "Server=localhost;Database=TestDb;User Id=test;Password=test;");

                    // Add queue storage configuration
                    builder.UseSetting(
                        "AzureQueueStorage:ConnectionString",
                        "UseDevelopmentStorage=true");
                });

        var client = factory.CreateClient();

        var request = new
        {
            Name = "",
            Email = "not-an-email"
        };

        var response =
            await client.PostAsJsonAsync("/api/customers", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}