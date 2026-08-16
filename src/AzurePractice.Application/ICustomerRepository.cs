using AzurePractice.Domain;

namespace AzurePractice.Application;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();

    Task<Customer?> GetByIdAsync(int id);

    Task<Customer> AddAsync(Customer customer);
}