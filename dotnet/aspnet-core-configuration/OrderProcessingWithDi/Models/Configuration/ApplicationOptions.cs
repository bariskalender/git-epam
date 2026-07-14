namespace OrderProcessingWithDi.Models.Configuration;

public class ApplicationOptions
{
    public const string SectionName = "Application";

    public string ApplicationName { get; set; } = "Order Processing API";

    public string Version { get; set; } = "1.0.0";

    public string Environment { get; set; } = "Development";
}
