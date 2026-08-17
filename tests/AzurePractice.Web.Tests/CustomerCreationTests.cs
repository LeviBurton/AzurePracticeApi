using System.Net;
using System.Net.Http.Json;
using AzurePractice.Application;
using AzurePractice.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using AzurePractice.Web.Dtos.Customers;
using Microsoft.AspNetCore.Hosting;

namespace AzurePractice.Web.Tests;

public class CustomerCreationTests
{
    [Fact]
    public async Task CreateCustomer_WithValidInput_ReturnsCreatedCustomer()
    {
        await using var factory =
            new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseSetting(
                        "ConnectionStrings:DefaultConnection",
                        "Server=localhost;Database=TestDb;User Id=test;Password=test;");

                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll<ICustomerService>();
                        services.AddScoped<ICustomerService, FakeCustomerService>();
                    });
                });

        var client = factory.CreateClient();

        var request = new
        {
            Name = "Integration Test Customer",
            Email = "integration@example.com"
        };

        var response =
            await client.PostAsJsonAsync("/api/customers", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var customer =
            await response.Content.ReadFromJsonAsync<CustomerResponse>();

        Assert.NotNull(customer);
        Assert.Equal(123, customer.Id);
        Assert.Equal("Integration Test Customer", customer.Name);
        Assert.Equal("integration@example.com", customer.Email);
    }

    private sealed class FakeCustomerService : ICustomerService
    {
        public Task<List<Customer>> GetAllAsync()
        {
            return Task.FromResult(new List<Customer>());
        }

        public Task<Customer?> GetByIdAsync(int id)
        {
            return Task.FromResult<Customer?>(null);
        }

        public Task<Customer> CreateAsync(Customer customer)
        {
            customer.Id = 123;
            customer.CreatedUtc = DateTime.UtcNow;

            return Task.FromResult(customer);
        }
    }
}