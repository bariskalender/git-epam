namespace Northwind.Services.EntityFramework.Entities;

public class Shipper
{
    public long ShipperId { get; set; }

    public string CompanyName { get; set; } = default!;

    public string? Phone { get; set; }

    public ICollection<Order> Orders { get; } = new List<Order>();
}
