using AzurePractice.Domain;

namespace AzurePractice.Application;

public interface ICustomerService
{
    Task<List<Customer>> GetAllAsync();

    Task<Customer?> GetByIdAsync(int id);

    Task<Customer> CreateAsync(Customer customer);
}