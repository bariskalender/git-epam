namespace Northwind.Services.EntityFramework.Entities;

public class Category
{
    public long CategoryId { get; set; }

    public string CategoryName { get; set; } = default!;

    public string? Description { get; set; }

    public ICollection<Product> Products { get; } = new List<Product>();
}
