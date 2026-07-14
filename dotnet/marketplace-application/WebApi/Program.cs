using AutoMapper;
using Business;
using Business.Interfaces;
using Business.Services;
using Business.Validation;
using Data.Data;
using Data.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure basic MVC services
builder.Services.AddControllers();

// Configure database context based on operating system
builder.Services.AddDbContext<TradeMarketDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register data seeding service
builder.Services.AddScoped<DataSeeder>();

// Register Unit of Work pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Configure AutoMapper for entity-model mapping
builder.Services.AddAutoMapper(
    cfg => { },
    typeof(AutomapperProfile).Assembly);

// Register business layer services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<IStatisticService, StatisticService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider
        .GetRequiredService<DataSeeder>();

    seeder.Seed();
}

// Configure development environment features
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = feature?.Error;

        context.Response.ContentType = "application/json";

        if (exception is MarketException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(new
            {
                error = exception.Message
            });

            return;
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(new
        {
            error = "Internal Server Error"
        });
    });
});

// Configure middleware pipeline
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// 🚨 CRITICAL: DO NOT DELETE THIS CLASS!
public partial class Program
{
    protected Program()
    {
    }
}
