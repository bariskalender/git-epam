namespace StoreBLL.Models;

public class OrderDetailModel
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public decimal Price { get; set; }

    public int ProductAmount { get; set; }
}