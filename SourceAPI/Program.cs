using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using SourceAPI.Data;
using SourceAPI.Mappings;
using SourceAPI.Repositories;
using SourceAPI.Repositories.Interfaces;
using SourceAPI.Services;
using SourceAPI.Services.Interfaces;

// NLogの初期設定
var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
logger.Debug("Application starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // NLogをロギングプロバイダーとして使用
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();

    // Add Mapster
    MappingConfig.RegisterMappings();
    builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
    builder.Services.AddScoped<IMapper, ServiceMapper>();

    // Add DbContext
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db"));

    // Add Repositories
    builder.Services.AddScoped<IProductRepository, ProductRepository>();

    // Add Services
    builder.Services.AddScoped<IProductService, ProductService>();

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
            var forecast = Enumerable.Range(1, 5).Select(index =>
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

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    // アプリケーション起動時のエラーをログに記録
    logger.Error(ex, "Application stopped because of exception");
    throw;
}
finally
{
    // NLogをシャットダウン
    LogManager.Shutdown();
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}