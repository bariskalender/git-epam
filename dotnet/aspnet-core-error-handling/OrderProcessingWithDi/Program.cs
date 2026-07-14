using OrderProcessingWithDi.Middleware;
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

        builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();

        builder.Services.AddScoped<IOrderService, OrderService>();

        builder.Services.AddScoped<IOrderValidator, OrderValidator>();

        builder.Services.AddTransient<PricingService>();

        builder.Services.AddTransient<SimplePricingService>();

        builder.Services.AddTransient<IPricingService, PricingService>();

        builder.Services.AddSingleton<IPricingServiceFactory, PricingServiceFactory>();

        builder.Services.AddSingleton<ILifetimeDemoService, LifetimeDemoService>();

        builder.Services.AddScoped<ILifetimeDemoService, LifetimeDemoService>();

        builder.Services.AddTransient<ILifetimeDemoService, LifetimeDemoService>();

        var app = builder.Build();

        app.UseErrorHandling();

        app.UseDependencyInjectionDemo();

        app.MapPost(
            "/orders",
            async (
                string productId,
                int quantity,
                decimal unitPrice,
                IOrderService orderService) =>
            {
                var result =
                    await orderService.ProcessOrderAsync(
                        productId,
                        quantity,
                        unitPrice);

                return Results.Ok(result);
            });

        app.MapGet(
            "/orders",
            (IOrderRepository repository) =>
            {
                return Results.Ok(repository.GetAll());
            });

        app.MapGet(
            "/factory-demo",
            (
                string? serviceType,
                IPricingServiceFactory factory) =>
            {
                try
                {
                    var pricingService =
                        factory.CreatePricingService(serviceType);

                    var total =
                        pricingService.CalculateTotal(10, 6);

                    return Results.Ok(new
                    {
                        serviceType = serviceType ?? "standard",
                        total
                    });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new
                    {
                        error = ex.Message
                    });
                }
            });

        app.MapGet(
            "/di-demo",
            (
                ILifetimeDemoService singleton,
                ILifetimeDemoService scoped,
                ILifetimeDemoService transient) =>
            {
                return Results.Ok(new
                {
                    singleton = new
                    {
                        singleton.InstanceId,
                        singleton.CreatedAt
                    },
                    scoped = new
                    {
                        scoped.InstanceId,
                        scoped.CreatedAt
                    },
                    transient = new
                    {
                        transient.InstanceId,
                        transient.CreatedAt
                    }
                });
            });

        var ordersGroup =
            app.MapGroup("/api/v1/orders")
                .WithTags("Orders API v1");

        ordersGroup.MapGet(
            "",
            (IOrderRepository repository) =>
            {
                return Results.Ok(repository.GetAll());
            });

        ordersGroup.MapGet(
            "/product/{productId}",
            (string productId, IOrderRepository repository) =>
            {
                var orders =
                    repository.GetAll()
                        .Where(x => x.ProductId == productId)
                        .ToList();

                return Results.Ok(orders);
            });

        ordersGroup.MapGet(
            "/total-range",
            (
                decimal minTotal,
                decimal maxTotal,
                IOrderRepository repository) =>
            {
                var orders =
                    repository.GetAll()
                        .Where(x =>
                            x.Total >= minTotal &&
                            x.Total <= maxTotal)
                        .ToList();

                return Results.Ok(orders);
            });

        ordersGroup.MapGet(
            "/search",
            (
                string? productId,
                decimal? minTotal,
                decimal? maxTotal,
                int? limit,
                IOrderRepository repository) =>
            {
                var query =
                    repository.GetAll().AsQueryable();

                if (!string.IsNullOrWhiteSpace(productId))
                {
                    query =
                        query.Where(x =>
                            x.ProductId == productId);
                }

                if (minTotal.HasValue)
                {
                    query =
                        query.Where(x =>
                            x.Total >= minTotal.Value);
                }

                if (maxTotal.HasValue)
                {
                    query =
                        query.Where(x =>
                            x.Total <= maxTotal.Value);
                }

                if (limit.HasValue)
                {
                    query =
                        query.Take(limit.Value);
                }

                return Results.Ok(query.ToList());
            });

        ordersGroup.MapGet(
            "/recent/{days:int}",
            (int days, IOrderRepository repository) =>
            {
                if (days <= 0 || days > 365)
                {
                    return Results.NotFound();
                }

                return Results.Ok(repository.GetAll());
            });

        ordersGroup.MapGet(
            "/statistics",
            (IOrderRepository repository) =>
            {
                var orders =
                    repository.GetAll().ToList();

                var statistics = new
                {
                    totalOrders = orders.Count,
                    totalRevenue = orders.Sum(x => x.Total),
                    averageOrderTotal =
                        orders.Count > 0
                            ? orders.Average(x => x.Total)
                            : 0m
                };

                return Results.Ok(statistics);
            });

        ordersGroup.MapGet(
            "/{orderId:int}",
            (int orderId, IOrderRepository repository) =>
            {
                var order = repository.GetById(orderId);

                if (order == null)
                {
                    throw new OrderNotFoundException(orderId);
                }

                return Results.Ok(order);
            });

        app.Run();
    }
}
