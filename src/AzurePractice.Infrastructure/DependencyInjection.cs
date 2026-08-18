using AzurePractice.Application;
using Azure.Identity;
using Azure.Storage.Queues;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AzurePractice.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sqlOptions => sqlOptions.EnableRetryOnFailure()));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
  
        return services;
    }

    public static IServiceCollection AddQueueMessaging(
        this IServiceCollection services,
        string queueName,
        string? connectionString,
        string? queueServiceUri)
    {
        QueueClient queueClient;

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            // Local development: Azurite or a storage connection string.
            queueClient = new QueueClient(
                connectionString,
                queueName);
        }
        else if (!string.IsNullOrWhiteSpace(queueServiceUri))
        {
            // Azure: authenticate with Managed Identity.
            var queueUri = new Uri(
                $"{queueServiceUri.TrimEnd('/')}/{queueName}");

            queueClient = new QueueClient(
                queueUri,
                new DefaultAzureCredential());
        }
        else
        {
            throw new InvalidOperationException(
                "Queue storage configuration was not found.");
        }

        services.AddSingleton(queueClient);
        services.AddScoped<ICustomerQueuePublisher, CustomerQueuePublisher>();

        return services;
    }
}