using Microsoft.Extensions.Logging.Abstractions;
using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Models.Exceptions;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly IPricingService pricingService;
    private readonly IOrderRepository repository;
    private readonly IOrderValidator validator;
    private readonly ILogger<OrderService> logger;

    public OrderService(IPricingService pricingService, IOrderRepository repository, IOrderValidator validator)
        : this(pricingService, repository, validator, NullLogger<OrderService>.Instance)
    {
    }

    public OrderService(
        IPricingService pricingService,
        IOrderRepository repository,
        IOrderValidator validator,
        ILogger<OrderService> logger)
    {
        this.pricingService = pricingService;
        this.repository = repository;
        this.validator = validator;
        this.logger = logger;
    }

    public async Task<OrderResult> ProcessOrderAsync(string productId, int quantity, decimal unitPrice)
    {
        this.logger.LogInformation(
            "Processing order for product {ProductId} with quantity {Quantity}",
            productId,
            quantity);

        try
        {
            this.ValidateOrder(productId, quantity, unitPrice);

            var total = this.pricingService.CalculateTotal(unitPrice, quantity);
            var result = new OrderResult(productId, quantity, total);

            await this.repository.SaveAsync(result);

            this.logger.LogInformation(
                "Order processed successfully. ProductId: {ProductId}, Total: {Total}",
                productId,
                total);

            return result;
        }
        catch (InvalidOrderException ex)
        {
            this.logger.LogWarning(
                "Order validation failed: {ErrorMessage}",
                ex.Message);

            throw;
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "Error processing order: {ErrorMessage}",
                ex.Message);

            throw;
        }
    }

    private void ValidateOrder(string productId, int quantity, decimal unitPrice)
    {
        var validationResult = this.validator.Validate(productId, quantity, unitPrice);

        if (!validationResult.IsValid)
        {
            throw new InvalidOrderException(validationResult.ErrorMessage ?? "Order validation failed");
        }
    }
}
