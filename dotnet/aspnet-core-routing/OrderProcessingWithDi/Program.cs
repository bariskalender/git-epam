using OrderProcessingWithDi.Services.Implementations;
using OrderProcessingWithDi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderValidator, OrderValidator>();
builder.Services.AddTransient<IPricingService, PricingService>();
builder.Services.AddSingleton<IPricingServiceFactory, PricingServiceFactory>();
builder.Services.AddTransient<ILifetimeDemoService, LifetimeDemoService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        context.Response.Headers["X-Singleton-Id"] = Guid.NewGuid().ToString();
        return Task.CompletedTask;
    });

    await next();
});

app.MapPost("/orders", async (string productId, int quantity, decimal unitPrice, IOrderService service) =>
{
    try
    {
        var result = await service.ProcessOrderAsync(productId, quantity, unitPrice);
        return Results.Ok(result);
    }
    catch (ArgumentException)
    {
        return Results.StatusCode(500);
    }
    catch (InvalidOperationException)
    {
        return Results.StatusCode(500);
    }
});

app.MapGet("/orders", (IOrderRepository repo) =>
{
    return Results.Ok(repo.GetAll());
});

app.MapGet("/orders/{orderId:int}", (int orderId, IOrderRepository repo) =>
{
    var order = repo.GetById(orderId);
    return order is null ? Results.NotFound() : Results.Ok(order);
});

app.MapGet("/orders/product/{productId:minlength(1)}", (string productId, IOrderRepository repo) =>
{
    var orders = repo.GetAll().Where(x => x.ProductId == productId);
    return Results.Ok(orders);
});

app.MapGet("/orders/range/{minTotal:decimal}/{maxTotal:decimal}", (decimal minTotal, decimal maxTotal, IOrderRepository repo) =>
{
    var orders = repo.GetAll().Where(x => x.Total >= minTotal && x.Total <= maxTotal);
    return Results.Ok(orders);
});

app.MapGet("/di-demo", (
    ILifetimeDemoService singleton,
    ILifetimeDemoService scoped,
    ILifetimeDemoService transient) =>
{
    return Results.Ok(new
    {
        instanceId = singleton.InstanceId,
        singleton = singleton.InstanceId,
        scoped = scoped.InstanceId,
        transient = transient.InstanceId
    });
});

app.MapGet("/factory-demo", (decimal price, int quantity, string? serviceType, IPricingServiceFactory factory) =>
{
    var service = factory.CreatePricingService(serviceType);
    var total = service.CalculateTotal(price, quantity);

    return Results.Ok(new
    {
        price,
        quantity,
        serviceType = serviceType ?? "standard",
        total
    });
});

var group = app.MapGroup("/api/v1/orders");

group.MapGet("/", (IOrderRepository repo) => Results.Ok(repo.GetAll()));

group.MapPost("/", async (string productId, int quantity, decimal unitPrice, IOrderService service) =>
{
    try
    {
        var result = await service.ProcessOrderAsync(productId, quantity, unitPrice);
        return Results.Ok(result);
    }
    catch (ArgumentException)
    {
        return Results.StatusCode(500);
    }
    catch (InvalidOperationException)
    {
        return Results.StatusCode(500);
    }
});

group.MapGet("/{orderId:int}", (int orderId, IOrderRepository repo) =>
{
    var order = repo.GetById(orderId);
    return order is null ? Results.NotFound() : Results.Ok(order);
});

group.MapGet("/product/{productId:minlength(1)}", (string productId, IOrderRepository repo) =>
{
    var orders = repo.GetAll().Where(x => x.ProductId == productId);
    return Results.Ok(orders);
});

group.MapGet("/range/{minTotal:decimal}/{maxTotal:decimal}", (decimal minTotal, decimal maxTotal, IOrderRepository repo) =>
{
    var orders = repo.GetAll().Where(x => x.Total >= minTotal && x.Total <= maxTotal);
    return Results.Ok(orders);
});

group.MapGet("/search", (string? productId, decimal? minTotal, decimal? maxTotal, int? limit, IOrderRepository repo) =>
{
    var orders = repo.GetAll().AsQueryable();

    if (!string.IsNullOrWhiteSpace(productId))
        orders = orders.Where(x => x.ProductId == productId);

    if (minTotal.HasValue)
        orders = orders.Where(x => x.Total >= minTotal);

    if (maxTotal.HasValue)
        orders = orders.Where(x => x.Total <= maxTotal);

    if (limit.HasValue)
        orders = orders.Take(limit.Value);

    return Results.Ok(orders.ToList());
});

group.MapGet("/recent/{days:int:range(1,30)}", (int days, IOrderRepository repo) =>
{
    return Results.Ok(repo.GetAll());
});

group.MapGet("/stats", (IOrderRepository repo) =>
{
    var orders = repo.GetAll();

    var totalOrders = orders.Count;
    var totalRevenue = orders.Sum(x => x.Total);
    var average = totalOrders == 0 ? 0 : totalRevenue / totalOrders;

    var mostOrdered = orders
        .GroupBy(x => x.ProductId)
        .OrderByDescending(g => g.Sum(x => x.Quantity))
        .FirstOrDefault()?.Key;

    return Results.Ok(new
    {
        totalOrders,
        totalRevenue,
        averageOrderTotal = average,
        mostOrderedProductId = mostOrdered
    });
});

group.MapGet("/{id}", (string id, IOrderRepository repo) =>
{
    if (int.TryParse(id, out var intId))
    {
        var order = repo.GetById(intId);
        if (order != null)
            return Results.Ok(order);
    }

    var orders = repo.GetAll().Where(x => x.ProductId == id);

    if (orders.Any())
        return Results.Ok(orders);

    return Results.NotFound();
});

app.Run();

public partial class Program
{
    protected Program() { }
}
