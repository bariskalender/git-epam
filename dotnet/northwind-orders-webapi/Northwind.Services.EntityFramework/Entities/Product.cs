namespace Northwind.Services.EntityFramework.Entities;

public class Product
{
    public long ProductId { get; set; }

    public string ProductName { get; set; } = default!;

    public long SupplierId { get; set; }

    public Supplier Supplier { get; set; } = default!;

    public long CategoryId { get; set; }

    public Category Category { get; set; } = default!;

    public string? QuantityPerUnit { get; set; }

    public double UnitPrice { get; set; }

    public long UnitsInStock { get; set; }

    public long UnitsOnOrder { get; set; }

    public long ReorderLevel { get; set; }

    public int Discontinued { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
