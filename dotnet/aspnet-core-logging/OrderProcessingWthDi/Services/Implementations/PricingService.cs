using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging.Abstractions;
using OrderProcessingWithDi.Models.Configuration;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

public class PricingService : IPricingService
{
    private readonly PricingOptions options;
    private readonly ILogger<PricingService> logger;

    public PricingService(IOptions<PricingOptions> options)
        : this(options, NullLogger<PricingService>.Instance)
    {
    }

    public PricingService(IOptions<PricingOptions> options, ILogger<PricingService> logger)
    {
        this.options = options.Value;
        this.logger = logger;
    }

    public decimal CalculateTotal(decimal basePrice, int quantity)
    {
        this.logger.LogDebug(
            "Calculating total for price {Price} and quantity {Quantity}",
            basePrice,
            quantity);

        var originalTotal = basePrice * quantity;
        var total = originalTotal;

        if (quantity > this.options.DiscountThreshold && total >= this.options.MinimumOrderValue)
        {
            total *= (1 - this.options.DiscountPercentage);

            this.logger.LogInformation(
                "Discount applied. Original total: {OriginalTotal}, Discount: {DiscountPercentage}%, Final total: {FinalTotal}",
                originalTotal,
                this.options.DiscountPercentage * 100,
                total);
        }
        else
        {
            this.logger.LogDebug(
                "No discount applied. Total: {Total}",
                total);
        }

        return total;
    }
}
