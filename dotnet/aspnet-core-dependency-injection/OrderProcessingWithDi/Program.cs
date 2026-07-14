using OrderProcessingWithDi.Middleware;
using OrderProcessingWithDi.Services.Implementations;
using OrderProcessingWithDi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderValidator, OrderValidator>();

builder.Services.AddTransient<IPricingService, PricingService>();

builder.Services.AddSingleton<IPricingServiceFactory, PricingServiceFactory>();

builder.Services.AddSingleton<ILifetimeDemoService>(
    sp => new LifetimeDemoService());

builder.Services.AddScoped<ILifetimeDemoService>(
    sp => new LifetimeDemoService());

builder.Services.AddTransient<ILifetimeDemoService>(
    sp => new LifetimeDemoService());

var app = builder.Build();

app.UseDependencyInjectionDemo();

app.MapPost(
    "/orders",
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
    })
    .WithName("CreateOrder")
    .WithTags("Orders");

app.MapGet(
    "/orders",
    (IOrderRepository repository) =>
    {
        var orders = repository.GetAll();

        return Results.Ok(orders);
    })
    .WithName("GetAllOrders")
    .WithTags("Orders");

app.MapGet(
    "/di-demo",
    (
        ILifetimeDemoService singletonService,
        ILifetimeDemoService scopedService,
        ILifetimeDemoService transientService) =>
    {
        return Results.Ok(new
        {
            message = "Dependency Injection Lifetime Demonstration",
            singleton = new
            {
                instanceId = singletonService.InstanceId,
                createdAt = singletonService.CreatedAt
            },
            scoped = new
            {
                instanceId = scopedService.InstanceId,
                createdAt = scopedService.CreatedAt
            },
            transient = new
            {
                instanceId = transientService.InstanceId,
                createdAt = transientService.CreatedAt
            },
            explanation = new
            {
                singleton = "Same instance for entire application lifetime",
                scoped = "Same instance per request",
                transient = "New instance every resolution"
            }
        });
    })
    .WithName("DependencyInjectionDemo")
    .WithTags("DI Demo");

app.MapGet(
    "/factory-demo",
    (
        IPricingServiceFactory factory,
        decimal price,
        int quantity,
        string? serviceType) =>
    {
        var pricingService =
            factory.CreatePricingService(serviceType);

        decimal total =
            pricingService.CalculateTotal(price, quantity);

        return Results.Ok(new
        {
            message = "Factory Pattern Demonstration",
            serviceType = serviceType ?? "standard",
            price,
            quantity,
            total,
            explanation =
                "Factory creates pricing services dynamically"
        });
    })
    .WithName("FactoryDemo")
    .WithTags("DI Demo");

app.Run();

public partial class Program
{
}
