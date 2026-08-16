using AzurePractice.Domain;
using Microsoft.EntityFrameworkCore;

namespace AzurePractice.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
}