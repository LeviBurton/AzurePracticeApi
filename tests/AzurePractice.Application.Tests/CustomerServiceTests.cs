using AzurePractice.Application;
using AzurePractice.Domain;

namespace AzurePractice.Application.Tests;

public class CustomerServiceTests
{
    [Fact]
    public async Task CreateAsync_AddsCustomerThroughRepository()
    {
        var repository = new FakeCustomerRepository();
        var service = new CustomerService(repository);

        var customer = new Customer
        {
            Name = "Test Customer",
            Email = "test@example.com"
        };

        var result = await service.CreateAsync(customer);

        Assert.Equal("Test Customer", result.Name);
        Assert.Equal("test@example.com", result.Email);
        Assert.Single(repository.Customers);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMatchingCustomer()
    {
        var repository = new FakeCustomerRepository();

        repository.Customers.Add(new Customer
        {
            Id = 1,
            Name = "Levi",
            Email = "levi@example.com"
        });

        var service = new CustomerService(repository);

        var result = await service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Levi", result.Name);
    }
    
    private sealed class FakeCustomerRepository : ICustomerRepository
    {
        public List<Customer> Customers { get; } = [];

        public Task<List<Customer>> GetAllAsync()
        {
            return Task.FromResult(Customers);
        }

        public Task<Customer?> GetByIdAsync(int id)
        {
            return Task.FromResult(
                Customers.FirstOrDefault(c => c.Id == id));
        }

        public Task<Customer> AddAsync(Customer customer)
        {
            customer.Id = Customers.Count + 1;
            Customers.Add(customer);

            return Task.FromResult(customer);
        }
    }
}