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

// Add CORS policy to allow your Azure app's URL
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAzureApp", builder =>
    {
        builder.WithOrigins("https://lemon-plant-09d34e60f.6.azurestaticapps.net/")  // Replace with the actual Azure app URL
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Enable Swagger UI in both development and production environments
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS policy here
app.UseCors("AllowAzureApp");

app.UseHttpsRedirection();
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Run the application
app.Run();
