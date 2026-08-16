using AzurePractice.Application;
using AzurePractice.Domain;
using Microsoft.EntityFrameworkCore;

namespace AzurePractice.Infrastructure;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;

    public CustomerRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _db.Customers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _db.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer> AddAsync(Customer customer)
    {
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();

        return customer;
    }
}