using AzurePractice.Domain;

namespace AzurePractice.Application;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Task<List<Customer>> GetAllAsync()
    {
        return _customerRepository.GetAllAsync();
    }

    public Task<Customer?> GetByIdAsync(int id)
    {
        return _customerRepository.GetByIdAsync(id);
    }

    public Task<Customer> CreateAsync(Customer customer)
    {
        return _customerRepository.AddAsync(customer);
    }
}