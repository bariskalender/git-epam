namespace Business.Models;

public class ProductModel
{
    public int Id { get; set; }

    public int ProductCategoryId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public ICollection<int> ReceiptDetailIds { get; set; } = new List<int>();
}
