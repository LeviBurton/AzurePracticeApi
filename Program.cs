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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

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
		Message = "Hello from Azure!",
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

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
