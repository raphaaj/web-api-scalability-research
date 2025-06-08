using B3.QuotationHistories.Application.Interfaces;
using B3.QuotationHistories.Application.UseCases.GetPaperQuotationHistoriesUseCase;
using B3.QuotationHistories.Application.UseCases.GetTopNAssetsWithHighestNegotiatedVolumeUseCase;
using B3.QuotationHistories.Infrastructure.Persistence;
using B3.QuotationHistories.Infrastructure.Repositories;
using B3.QuotationHistories.WebApi.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var b3QuotationHistories2024ConnectionString =
    builder.Configuration.GetConnectionString("B3QuotationHistories2024Connection");

if (string.IsNullOrWhiteSpace(b3QuotationHistories2024ConnectionString))
    throw new Exception("Environment variable ConnectionStrings__B3QuotationHistories2024Connection is not set.");

// Add services to the container.
builder.Services.AddDbContext<B3QuotationHistoriesDbContext>(options =>
    options.UseNpgsql(b3QuotationHistories2024ConnectionString));

builder.Services.AddHealthChecks()
    .AddDbContextCheck<B3QuotationHistoriesDbContext>("B3QuotationHistoriesDb-EntityFrameworkCore")
    .AddNpgSql(b3QuotationHistories2024ConnectionString, name: "B3QuotationHistoriesDb-Infrastructure",
        timeout: TimeSpan.FromSeconds(5));

// Add repositories to the container.
builder.Services.AddScoped<IQuotationHistoryRepository, QuotationHistoryRepository>();
builder.Services.AddScoped<GetPaperQuotationHistoriesUseCase>();
builder.Services.AddScoped<GetTopNAssetsWithHighestNegotiatedVolumeUseCase>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Disable HTTPS redirection for now
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks();

app.Run();