using Microsoft.Extensions.Options;
using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Models.Configuration;
using OrderProcessingWithDi.Models.Exceptions;
using OrderProcessingWithDi.Services.Implementations;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi;

public class Program
{
    private static readonly string SingletonInstanceId = Guid.NewGuid().ToString();

    protected Program()
    {
    }

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<PricingOptions>(
            builder.Configuration.GetSection(PricingOptions.SectionName));

        builder.Services.Configure<OrderProcessingOptions>(
            builder.Configuration.GetSection(OrderProcessingOptions.SectionName));

        builder.Services.Configure<ApplicationOptions>(
            builder.Configuration.GetSection(ApplicationOptions.SectionName));

        builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        builder.Services.AddScoped<IOrderValidator, OrderValidator>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddTransient<IPricingService, PricingService>();
        builder.Services.AddSingleton<IPricingServiceFactory, PricingServiceFactory>();

        var app = builder.Build();

        app.Use(async (context, next) =>
        {
            try
            {
                await next();
            }
            catch (InvalidOrderException ex)
            {
                context.Response.StatusCode = 400;

                await context.Response.WriteAsJsonAsync(new ErrorResponse
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "Invalid Order",
                    Status = 400,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                });
            }
            catch (OrderNotFoundException ex)
            {
                context.Response.StatusCode = 404;

                await context.Response.WriteAsJsonAsync(new ErrorResponse
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = "Order Not Found",
                    Status = 404,
                    Detail = ex.Message,
                    Instance = context.Request.Path,
                    Extensions = new Dictionary<string, object>
                    {
                        ["orderId"] = ex.OrderId
                    }
                });
            }
        });

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

        app.MapGet("/di-demo",
            (HttpContext context) =>
            {
                context.Response.Headers["X-DI-Singleton-Instance"] =
                    SingletonInstanceId;

                return Results.Ok(new
                {
                    singleton = new
                    {
                        instanceId = SingletonInstanceId
                    },
                    scoped = new
                    {
                        instanceId = Guid.NewGuid()
                    },
                    transient = new
                    {
                        instanceId = Guid.NewGuid()
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
                var pricingService =
                    factory.CreatePricingService(serviceType);

                var total =
                    pricingService.CalculateTotal(price, quantity);

                return Results.Ok(new
                {
                    serviceType = serviceType ?? "standard",
                    total
                });
            });

        var api = app.MapGroup("/api/v1");

        api.MapGet("/orders",
            (IOrderRepository repository) =>
            {
                return Results.Ok(repository.GetAll());
            });

        api.MapGet("/orders/{orderId:int}",
            (int orderId, IOrderRepository repository) =>
            {
                var order = repository.GetById(orderId);

                if (order is null)
                {
                    throw new OrderNotFoundException(orderId);
                }

                return Results.Ok(order);
            });

        api.MapGet("/orders/product/{productId}",
            (string productId, IOrderRepository repository) =>
            {
                var orders = repository.GetAll()
                    .Where(x => x.ProductId == productId);

                return Results.Ok(orders);
            });

        api.MapGet("/orders/range/{minTotal:decimal}/{maxTotal:decimal}",
            (
                decimal minTotal,
                decimal maxTotal,
                IOrderRepository repository) =>
            {
                var orders = repository.GetAll()
                    .Where(x =>
                        x.Total >= minTotal &&
                        x.Total <= maxTotal);

                return Results.Ok(orders);
            });

        api.MapGet("/orders/search",
            (
                string? productId,
                decimal? minTotal,
                decimal? maxTotal,
                int? limit,
                IOrderRepository repository) =>
            {
                var orders = repository.GetAll().AsEnumerable();

                if (!string.IsNullOrWhiteSpace(productId))
                {
                    orders = orders.Where(x => x.ProductId == productId);
                }

                if (minTotal.HasValue)
                {
                    orders = orders.Where(x => x.Total >= minTotal.Value);
                }

                if (maxTotal.HasValue)
                {
                    orders = orders.Where(x => x.Total <= maxTotal.Value);
                }

                if (limit.HasValue)
                {
                    orders = orders.Take(limit.Value);
                }

                return Results.Ok(orders);
            });

        api.MapGet("/orders/recent/{days:int:range(1,30)}",
            (int days, IOrderRepository repository) =>
            {
                return Results.Ok(repository.GetAll());
            });

        api.MapGet("/orders/stats",
            (IOrderRepository repository) =>
            {
                var orders = repository.GetAll();

                return Results.Ok(new
                {
                    totalOrders = orders.Count,
                    totalRevenue = orders.Sum(x => x.Total),
                    averageOrderTotal =
                        orders.Count == 0
                            ? 0
                            : orders.Average(x => x.Total),

                    mostOrderedProductId =
                        orders
                            .GroupBy(x => x.ProductId)
                            .OrderByDescending(x => x.Count())
                            .FirstOrDefault()
                            ?.Key
                });
            });

        app.MapGet("/config/pricing",
            (IOptions<PricingOptions> options) =>
            {
                return Results.Ok(options.Value);
            })
            .WithName("GetPricingConfig")
            .WithTags("Configuration");

        app.MapGet("/config/order-processing",
            (IOptions<OrderProcessingOptions> options) =>
            {
                return Results.Ok(options.Value);
            })
            .WithName("GetOrderProcessingConfig")
            .WithTags("Configuration");

        app.MapGet("/config/application",
            (IOptions<ApplicationOptions> options) =>
            {
                return Results.Ok(options.Value);
            })
            .WithName("GetApplicationConfig")
            .WithTags("Configuration");

        app.MapGet("/config/all",
            (
                IOptions<PricingOptions> pricing,
                IOptions<OrderProcessingOptions> orderProcessing,
                IOptions<ApplicationOptions> application) =>
            {
                return Results.Ok(new
                {
                    pricing = pricing.Value,
                    orderProcessing = orderProcessing.Value,
                    application = application.Value
                });
            })
            .WithName("GetAllConfig")
            .WithTags("Configuration");

        app.MapGet("/config/raw",
            (IConfiguration configuration) =>
            {
                return Results.Ok(new
                {
                    pricing =
                        configuration
                            .GetSection("Pricing")
                            .Get<PricingOptions>(),

                    orderProcessing =
                        configuration
                            .GetSection("OrderProcessing")
                            .Get<OrderProcessingOptions>(),

                    application =
                        configuration
                            .GetSection("Application")
                            .Get<ApplicationOptions>()
                });
            })
            .WithName("GetRawConfig")
            .WithTags("Configuration");

        app.Run();
    }
}
