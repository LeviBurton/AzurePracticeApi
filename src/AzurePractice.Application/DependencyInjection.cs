using Microsoft.Extensions.DependencyInjection;

namespace AzurePractice.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }
}