using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<OrderResult> orders = new();

    public Task SaveAsync(OrderResult order)
    {
        this.orders.Add(order);
        return Task.CompletedTask;
    }

    public IReadOnlyList<OrderResult> GetAll()
    {
        return this.orders.AsReadOnly();
    }

    public OrderResult? GetById(int id)
    {
        if (id < 0 || id >= this.orders.Count)
        {
            return null;
        }

        return this.orders[id];
    }

    public void Clear()
    {
        this.orders.Clear();
    }
}
