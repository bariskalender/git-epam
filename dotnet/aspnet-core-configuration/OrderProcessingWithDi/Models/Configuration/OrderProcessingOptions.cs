namespace OrderProcessingWithDi.Models.Configuration;

public class OrderProcessingOptions
{
    public const string SectionName = "OrderProcessing";

    public int MaxQuantity { get; set; } = 1000;

    public decimal MaxOrderValue { get; set; } = 10000m;

    public bool EnableValidation { get; set; } = true;
}
