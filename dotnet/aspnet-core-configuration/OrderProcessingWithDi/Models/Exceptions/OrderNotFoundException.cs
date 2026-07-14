namespace OrderProcessingWithDi.Models.Exceptions;

public class OrderNotFoundException : Exception
{
    public int OrderId { get; }

    public OrderNotFoundException()
    {
    }

    public OrderNotFoundException(string message)
        : base(message)
    {
    }

    public OrderNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public OrderNotFoundException(int orderId)
        : base($"Order with ID {orderId} was not found.")
    {
        this.OrderId = orderId;
    }
}
