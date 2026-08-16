using System;
using Microsoft.EntityFrameworkCore;
namespace AzurePracticeApi;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
}
