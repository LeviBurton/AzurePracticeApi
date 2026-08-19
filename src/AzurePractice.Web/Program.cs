using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using AzurePractice.Infrastructure;
using AzurePractice.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(
        builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi(
        new[] { "User.Read" })
    .AddMicrosoftGraph()
    .AddInMemoryTokenCaches();

builder.Services.Configure<OpenIdConnectOptions>(
    OpenIdConnectDefaults.AuthenticationScheme,
    options =>
    {
        options.ResponseType = "code";
    });
    
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOpenApi();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' was not found.");

builder.Services.AddInfrastructure(connectionString);

var queueName =
    builder.Configuration["QueueStorage:QueueName"]
    ?? "customer-work-items";

var queueConnectionString =
    builder.Configuration["QueueStorage:ConnectionString"];

var queueServiceUri =
    builder.Configuration["QueueStorage:ServiceUri"];

builder.Services.AddQueueMessaging(
    queueName,
    queueConnectionString,
    queueServiceUri);

builder.Services.AddApplication();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanCreateCustomers", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("CustomerCreator");
    });
});

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapStaticAssets();
app.UseStaticFiles();


app.MapControllers();   

app.MapRazorComponents<AzurePractice.Web.Components.App>()
    .AddInteractiveServerRenderMode();


app.Run();
