using OrderProcessingWithDi.Middleware;
using OrderProcessingWithDi.Models.Configuration;
using OrderProcessingWithDi.Models.Exceptions;
using OrderProcessingWithDi.Services.Implementations;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi;

public class Program
{
    protected Program()
    {
    }

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // CONFIGURATION
        builder.Services.Configure<PricingOptions>(
            builder.Configuration.GetSection("Pricing"));

        // DI REGISTRATIONS
        builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();

        builder.Services.AddScoped<IOrderService, OrderService>();

        builder.Services.AddTransient<IPricingService, PricingService>();

        builder.Services.AddTransient<IOrderValidator, OrderValidator>();

        builder.Services.AddSingleton<IPricingServiceFactory, PricingServiceFactory>();

        var app = builder.Build();

        // MIDDLEWARE
        app.UseErrorHandling();

        // ============================================================================
        // BASIC ENDPOINTS
        // ============================================================================

        app.MapPost("/orders",
            async (
                string productId,
                int quantity,
                decimal unitPrice,
                IOrderService orderService) =>
            {
                var result = await orderService.ProcessOrderAsync(
                    productId,
                    quantity,
                    unitPrice);

                return Results.Ok(result);
            });

        app.MapGet("/orders",
            (IOrderRepository repository) =>
            {
                return Results.Ok(repository.GetAll());
            });

        app.MapGet("/orders/{id:int}",
            (int id, IOrderRepository repository) =>
            {
                var order = repository.GetById(id);

                if (order is null)
                {
                    throw new OrderNotFoundException(id);
                }

                return Results.Ok(order);
            });

        // ============================================================================
        // ROUTE GROUP
        // ============================================================================

        var api = app.MapGroup("/api/v1/orders");

        api.MapGet("/",
            (IOrderRepository repository) =>
            {
                return Results.Ok(repository.GetAll());
            });

        api.MapGet("/{id:int}",
            (int id, IOrderRepository repository) =>
            {
                var order = repository.GetById(id);

                if (order is null)
                {
                    throw new OrderNotFoundException(id);
                }

                return Results.Ok(order);
            });

        api.MapGet("/product/{productId:minlength(1)}",
            (string productId, IOrderRepository repository) =>
            {
                var result = repository
                    .GetAll()
                    .Where(x => x.ProductId == productId)
                    .ToList();

                return Results.Ok(result);
            });

        api.MapGet("/range/{minTotal:decimal}/{maxTotal:decimal}",
            (decimal minTotal, decimal maxTotal, IOrderRepository repository) =>
            {
                var result = repository
                    .GetAll()
                    .Where(x => x.Total >= minTotal && x.Total <= maxTotal)
                    .ToList();

                return Results.Ok(result);
            });

        api.MapGet("/search",
            (
                string? productId,
                decimal? minTotal,
                decimal? maxTotal,
                int? limit,
                IOrderRepository repository) =>
            {
                var query = repository.GetAll().AsEnumerable();

                if (!string.IsNullOrWhiteSpace(productId))
                {
                    query = query.Where(x => x.ProductId == productId);
                }

                if (minTotal.HasValue)
                {
                    query = query.Where(x => x.Total >= minTotal.Value);
                }

                if (maxTotal.HasValue)
                {
                    query = query.Where(x => x.Total <= maxTotal.Value);
                }

                if (limit.HasValue)
                {
                    query = query.Take(limit.Value);
                }

                return Results.Ok(query.ToList());
            });

        api.MapGet("/recent/{days:int:range(1,30)}",
            (int days, IOrderRepository repository) =>
            {
                return Results.Ok(repository.GetAll());
            });

        api.MapGet("/stats",
            (IOrderRepository repository) =>
            {
                var orders = repository.GetAll();

                var totalOrders = orders.Count;

                var totalRevenue = orders.Sum(x => x.Total);

                var averageOrderTotal =
                    totalOrders == 0
                        ? 0
                        : orders.Average(x => x.Total);

                var mostOrderedProductId =
                    orders
                        .GroupBy(x => x.ProductId)
                        .OrderByDescending(x => x.Count())
                        .Select(x => x.Key)
                        .FirstOrDefault();

                return Results.Ok(new
                {
                    totalOrders,
                    totalRevenue,
                    averageOrderTotal,
                    mostOrderedProductId
                });
            });

        // ============================================================================
        // DI DEMO
        // ============================================================================

        app.MapGet("/di-demo",
            (
                HttpContext context,
                IOrderRepository repository,
                IPricingService pricingService,
                IOrderValidator validator) =>
            {
                var singletonId = repository.GetHashCode().ToString();

                context.Response.Headers["X-DI-Singleton-Instance"] = singletonId;

                return Results.Ok(new
                {
                    singleton = singletonId,
                    scoped = validator.GetHashCode().ToString(),
                    transient = pricingService.GetHashCode().ToString(),
                    instanceId = Guid.NewGuid().ToString(),
                    createdAt = DateTime.UtcNow
                });
            })
            .WithName("GetDiDemo")
            .WithTags("DI");

        // ============================================================================
        // FACTORY DEMO
        // ============================================================================

        app.MapGet("/factory-demo",
            (
                decimal price,
                int quantity,
                string? serviceType,
                IPricingServiceFactory factory) =>
            {
                var service = factory.CreatePricingService(serviceType);

                var total = service.CalculateTotal(price, quantity);

                return Results.Ok(new
                {
                    serviceType = serviceType ?? "standard",
                    total
                });
            });

        // ============================================================================
        // ENVIRONMENT CONFIG
        // ============================================================================

        app.MapGet("/config/environment",
            (IConfiguration configuration, IWebHostEnvironment environment) =>
            {
                return Results.Ok(new
                {
                    environmentName = environment.EnvironmentName,
                    applicationName = environment.ApplicationName,
                    contentRootPath = environment.ContentRootPath,
                    webRootPath = environment.WebRootPath,
                    pricingFromEnv = configuration["Pricing:DiscountThreshold"],
                    appSettingsValue = configuration["Application:ApplicationName"]
                });
            })
            .WithName("GetEnvironmentConfig")
            .WithTags("Environment");

        app.Run();
    }
}
