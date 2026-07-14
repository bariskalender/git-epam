using Microsoft.Extensions.Options;
using OrderProcessingWithDi.Models.Configuration;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

public class PricingService : IPricingService
{
    private readonly PricingOptions options;

    public PricingService(IOptions<PricingOptions> options)
    {
        this.options = options.Value;
    }

    public decimal CalculateTotal(decimal basePrice, int quantity)
    {
        var total = basePrice * quantity;

        if (quantity > this.options.DiscountThreshold && total >= this.options.MinimumOrderValue)
        {
            total *= 1 - this.options.DiscountPercentage;
        }

        return total;
    }
}
