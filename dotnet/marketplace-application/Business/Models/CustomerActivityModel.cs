namespace Business.Models;

public class CustomerActivityModel
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public decimal ReceiptSum { get; set; }
}
