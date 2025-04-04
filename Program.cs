// Ensure all using statements are at the top
using TicketHubApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Register services for dependency injection
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inject configuration and custom services
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<AzureQueueService>();

var app = builder.Build();

// Enable Swagger UI in both development and production environments
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware setup
app.UseHttpsRedirection();
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Run the application
app.Run();
