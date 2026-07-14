using Microsoft.Extensions.Logging.Abstractions;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

public class OrderValidator : IOrderValidator
{
    private readonly ILogger<OrderValidator> logger;

    public OrderValidator()
        : this(NullLogger<OrderValidator>.Instance)
    {
    }

    public OrderValidator(ILogger<OrderValidator> logger)
    {
        this.logger = logger;
    }

    public (bool IsValid, string? ErrorMessage) Validate(string productId, int quantity, decimal unitPrice)
    {
        this.logger.LogDebug(
            "Validating order: ProductId={ProductId}, Quantity={Quantity}, UnitPrice={UnitPrice}",
            productId,
            quantity,
            unitPrice);

        if (string.IsNullOrWhiteSpace(productId))
        {
            this.logger.LogWarning(
                "Validation failed: {ValidationMessage}",
                "ProductId cannot be empty");

            return (false, "ProductId cannot be empty");
        }

        if (quantity <= 0)
        {
            this.logger.LogWarning(
                "Validation failed: {ValidationMessage}",
                "Quantity must be greater than 0");

            return (false, "Quantity must be greater than 0");
        }

        if (unitPrice <= 0)
        {
            this.logger.LogWarning(
                "Validation failed: {ValidationMessage}",
                "UnitPrice must be greater than 0");

            return (false, "UnitPrice must be greater than 0");
        }

        this.logger.LogDebug(
            "Validation passed for product {ProductId}",
            productId);

        return (true, null);
    }
}
