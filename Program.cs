using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using AzurePracticeApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add DbContext with SQL Server connection string
builder.Services.AddDbContext<AzurePracticeApi.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/api/customers", async (AppDbContext db) =>
{
    var customers = await db.Customers.ToListAsync();
    return customers;
});

app.MapPost("/api/customers", async (Customer customer, AppDbContext db) =>
{
    db.Customers.Add(customer);
    await db.SaveChangesAsync();

    return Results.Created($"/api/customers/{customer.Id}", customer);
});

app.MapGet("/api/azure", () =>
{
	return new 
	{ 
		Message = "Hello from Azure CI/CD!",
		Environment = app.Environment.EnvironmentName,
		Timestamp = DateTime.UtcNow
	};
});

app.MapGet("/api/config", (IConfiguration configuration) =>
{
    return new 
    { 
        AppliocationName = configuration["PracticeApp:Name"] ?? "Not configured",
    };
});


app.MapGet("/api/secret", async () =>
{
    var vaultUrl = $"https://{Environment.GetEnvironmentVariable("KEY_VAULT_NAME")}.vault.azure.net/";

    var client = new SecretClient(new Uri(vaultUrl), new DefaultAzureCredential());
    var secret = await client.GetSecretAsync("PracticeSecret");

    return new 
    { 
        Message = secret.Value.Value,
    };
});

app.Run();
