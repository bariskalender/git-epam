namespace OrderProcessingWithDi.Models.Configuration;

public class PricingOptions
{
    public const string SectionName = "Pricing";

    public int DiscountThreshold { get; set; } = 5;

    public decimal DiscountPercentage { get; set; } = 0.1m;

    public decimal MinimumOrderValue { get; set; } = 0m;
}
