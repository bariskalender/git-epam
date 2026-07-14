using Data.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApi.Tests;

/// <summary>
/// Custom factory for WebApi integration tests.
/// </summary>
public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    private static readonly string DatabaseName =
        $"TestDb_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<TradeMarketDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<TradeMarketDbContext>(options =>
            {
                options.UseInMemoryDatabase(DatabaseName);
                options.EnableSensitiveDataLogging();
            });

            var serviceProvider =
                services.BuildServiceProvider();

            using var scope =
                serviceProvider.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<TradeMarketDbContext>();

            var logger = scope.ServiceProvider
                .GetRequiredService<
                    ILogger<CustomWebApplicationFactory>>();

            try
            {
                db.Database.EnsureDeleted();

                db.Database.EnsureCreated();

                UnitTestDataHelper.SeedData(db);

                logger.LogInformation(
                    "Test database initialized successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error initializing test database");

                throw;
            }
        });
    }

    public HttpClient CreateTestClient()
    {
        var client = this.CreateClient();

        client.BaseAddress =
            new Uri("http://localhost");

        return client;
    }
}
