using B3.QuotationHistories.Application.Interfaces;
using B3.QuotationHistories.Application.UseCases.GetPaperQuotationHistoriesUseCase;
using B3.QuotationHistories.Application.UseCases.GetTopNAssetsWithHighestNegotiatedVolumeUseCase;
using B3.QuotationHistories.Infrastructure.Cache;
using B3.QuotationHistories.Infrastructure.Interfaces;
using B3.QuotationHistories.Infrastructure.Persistence;
using B3.QuotationHistories.Infrastructure.Repositories;
using B3.QuotationHistories.Infrastructure.Settings;
using B3.QuotationHistories.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Setup database connection
var b3QuotationHistories2024ConnectionString =
    builder.Configuration.GetConnectionString("B3QuotationHistories2024Connection");

if (string.IsNullOrWhiteSpace(b3QuotationHistories2024ConnectionString))
    throw new Exception("Environment variable ConnectionStrings__B3QuotationHistories2024Connection is not set.");

builder.Services.AddDbContext<B3QuotationHistoriesDbContext>(options =>
    options.UseNpgsql(b3QuotationHistories2024ConnectionString));

// Setup health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<B3QuotationHistoriesDbContext>("B3QuotationHistoriesDb-EntityFrameworkCore")
    .AddNpgSql(b3QuotationHistories2024ConnectionString, name: "B3QuotationHistoriesDb-Infrastructure",
        timeout: TimeSpan.FromSeconds(5));

// Setup app configurations
var cacheSettingsConfigurationSection = builder.Configuration.GetSection("CacheSettings");
var cacheSettings = cacheSettingsConfigurationSection.Get<CacheSettings>();
var useCacheForTopAssetsWithHighestNegotiatedVolume =
    cacheSettings?.UseCacheForTopAssetsWithHighestNegotiatedVolume ?? false;
builder.Services.Configure<CacheSettings>(cacheSettingsConfigurationSection);

// Add services to the container.
builder.Services.AddMemoryCache();

if (useCacheForTopAssetsWithHighestNegotiatedVolume)
{
    builder.Services.AddSingleton<IQuotationHistoryCacheService, QuotationHistoryCacheService>();
    builder.Services.AddScoped<QuotationHistoryRepository>();

    builder.Services.AddScoped<QuotationHistoryCacheInitializer>(serviceProvider =>
    {
        var logger = serviceProvider.GetRequiredService<ILogger<QuotationHistoryCacheInitializer>>();
        var cacheSettingsOptions = serviceProvider.GetRequiredService<IOptions<CacheSettings>>();
        var quotationHistoryRepository = serviceProvider.GetRequiredService<QuotationHistoryRepository>();
        var quotationHistoryCacheService = serviceProvider.GetRequiredService<IQuotationHistoryCacheService>();
        return new QuotationHistoryCacheInitializer(logger, cacheSettingsOptions, quotationHistoryRepository,
            quotationHistoryCacheService);
    });

    builder.Services.AddScoped<IQuotationHistoryRepository>(serviceProvider =>
    {
        var cacheSettingsOptions = serviceProvider.GetRequiredService<IOptions<CacheSettings>>();
        var baseQuotationHistoryRepository = serviceProvider.GetRequiredService<QuotationHistoryRepository>();
        var quotationHistoryCacheService = serviceProvider.GetRequiredService<IQuotationHistoryCacheService>();
        return new CachedQuotationHistoryRepository(cacheSettingsOptions, baseQuotationHistoryRepository,
            quotationHistoryCacheService);
    });
}
else
{
    builder.Services.AddScoped<IQuotationHistoryRepository, QuotationHistoryRepository>();
}

builder.Services.AddScoped<GetPaperQuotationHistoriesUseCase>();
builder.Services.AddScoped<GetTopNAssetsWithHighestNegotiatedVolumeUseCase>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (useCacheForTopAssetsWithHighestNegotiatedVolume)
{
    using var scope = app.Services.CreateScope();
    var quotationHistoryCacheInitializer =
        scope.ServiceProvider.GetRequiredService<QuotationHistoryCacheInitializer>();
    await quotationHistoryCacheInitializer.InitializeAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Disable HTTPS redirection
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks();

app.Run();