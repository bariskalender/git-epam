using Microsoft.Extensions.Options;
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

        builder.Services.Configure<PricingOptions>(
            builder.Configuration.GetSection("Pricing"));

        builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddTransient<IPricingService, PricingService>();
        builder.Services.AddTransient<IOrderValidator, OrderValidator>();
        builder.Services.AddSingleton<IPricingServiceFactory, PricingServiceFactory>();

        var app = builder.Build();

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.MapPost("/orders",
            async (
                string productId,
                int quantity,
                decimal unitPrice,
                IOrderService orderService) =>
            {
                var result = await orderService.ProcessOrderAsync(productId, quantity, unitPrice);
                return Results.Ok(result);
            });

        app.MapGet("/orders",
            (IOrderRepository repository) =>
            {
                return Results.Ok(repository.GetAll());
            });

        app.MapGet("/orders/{orderId:int}",
            (int orderId, IOrderRepository repository) =>
            {
                var order = repository.GetById(orderId);

                if (order is null)
                {
                    throw new OrderNotFoundException(orderId);
                }

                return Results.Ok(order);
            });

        var api = app.MapGroup("/api/v1/orders");

        api.MapGet("/",
            (IOrderRepository repository) =>
            {
                return Results.Ok(repository.GetAll());
            });

        api.MapGet("/{orderId:int}",
            (int orderId, IOrderRepository repository) =>
            {
                var order = repository.GetById(orderId);

                if (order is null)
                {
                    throw new OrderNotFoundException(orderId);
                }

                return Results.Ok(order);
            });

        api.MapGet("/product/{productId:minlength(1)}",
            (string productId, IOrderRepository repository) =>
            {
                var orders = repository.GetAll()
                    .Where(order => order.ProductId == productId)
                    .ToList();

                return Results.Ok(orders);
            });

        api.MapGet("/range/{minTotal:decimal}/{maxTotal:decimal}",
            (decimal minTotal, decimal maxTotal, IOrderRepository repository) =>
            {
                var orders = repository.GetAll()
                    .Where(order => order.Total >= minTotal && order.Total <= maxTotal)
                    .ToList();

                return Results.Ok(orders);
            });

        api.MapGet("/search",
            (
                string? productId,
                decimal? minTotal,
                decimal? maxTotal,
                int? limit,
                IOrderRepository repository) =>
            {
                IEnumerable<OrderProcessingWithDi.Models.OrderResult> query = repository.GetAll();

                if (!string.IsNullOrWhiteSpace(productId))
                {
                    query = query.Where(order => order.ProductId == productId);
                }

                if (minTotal.HasValue)
                {
                    query = query.Where(order => order.Total >= minTotal.Value);
                }

                if (maxTotal.HasValue)
                {
                    query = query.Where(order => order.Total <= maxTotal.Value);
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
                return Results.Ok(repository.GetAll().ToList());
            });

        api.MapGet("/stats",
            (IOrderRepository repository) =>
            {
                var orders = repository.GetAll().ToList();

                var mostOrderedProductId = orders
                    .GroupBy(order => order.ProductId)
                    .OrderByDescending(group => group.Count())
                    .Select(group => group.Key)
                    .FirstOrDefault();

                return Results.Ok(new
                {
                    totalOrders = orders.Count,
                    totalRevenue = orders.Sum(order => order.Total),
                    averageOrderTotal = orders.Count == 0 ? 0 : orders.Average(order => order.Total),
                    mostOrderedProductId
                });
            });

        app.MapGet("/di-demo",
            (
                HttpContext context,
                IOrderRepository repository,
                IOrderService orderService,
                IPricingService pricingService) =>
            {
                var singletonInstanceId = repository.GetHashCode().ToString();

                context.Response.Headers["X-DI-Singleton-Instance"] = singletonInstanceId;

                return Results.Ok(new
                {
                    singleton = new
                    {
                        instanceId = singletonInstanceId
                    },
                    scoped = new
                    {
                        instanceId = orderService.GetHashCode().ToString()
                    },
                    transient = new
                    {
                        instanceId = pricingService.GetHashCode().ToString()
                    }
                });
            });

        app.MapGet("/factory-demo",
            (
                decimal price,
                int quantity,
                string? serviceType,
                IPricingServiceFactory factory) =>
            {
                var pricingService = factory.CreatePricingService(serviceType ?? "standard");
                var total = pricingService.CalculateTotal(price, quantity);

                return Results.Ok(new
                {
                    serviceType = serviceType ?? "standard",
                    total
                });
            });

        app.MapGet("/config/environment",
            (
                IWebHostEnvironment environment,
                IConfiguration configuration) =>
            {
                return Results.Ok(new
                {
                    environmentName = environment.EnvironmentName,
                    applicationName = "OrderProcessingWithDi",
                    contentRootPath = environment.ContentRootPath,
                    webRootPath = environment.WebRootPath,
                    pricingFromEnv = configuration["Pricing:DiscountPercentage"]
                });
            });

        app.MapGet("/logging/demo",
            (ILogger<Program> logger) =>
            {
                logger.LogTrace("This is a Trace message");
                logger.LogDebug("This is a Debug message");
                logger.LogInformation("This is an Information message");
                logger.LogWarning("This is a Warning message");
                logger.LogError("This is an Error message");
                logger.LogCritical("This is a Critical message");

                return Results.Ok(new
                {
                    Message = "Check application logs to see different log levels"
                });
            })
            .WithName("LoggingDemo")
            .WithTags("Logging");

        app.MapGet("/logging/structured",
            (ILogger<Program> logger) =>
            {
                var userId = "user123";
                var action = "GetOrders";
                var timestamp = DateTime.UtcNow;

                logger.LogInformation(
                    "Structured log example: UserId={UserId}, Action={Action}, Timestamp={Timestamp}",
                    userId,
                    action,
                    timestamp);

                return Results.Ok(new
                {
                    userId,
                    action,
                    timestamp
                });
            })
            .WithName("StructuredLoggingDemo")
            .WithTags("Logging");

        app.MapGet("/logging/scopes",
            (ILogger<Program> logger) =>
            {
                using (logger.BeginScope("RequestId: {RequestId}", Guid.NewGuid()))
                {
                    logger.LogInformation("Processing request");

                    using (logger.BeginScope("Operation: {Operation}", "GetOrders"))
                    {
                        logger.LogInformation("Executing operation");
                    }

                    logger.LogInformation("Request completed");
                }

                return Results.Ok(new
                {
                    Message = "Check logs for scoped logging demonstration"
                });
            })
            .WithName("LoggingScopesDemo")
            .WithTags("Logging");

        app.Run();
    }
}
