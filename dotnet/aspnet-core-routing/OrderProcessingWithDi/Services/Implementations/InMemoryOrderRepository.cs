using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<OrderResult> orders = new();

    public Task SaveAsync(OrderResult order)
    {
        orders.Add(order);
        return Task.CompletedTask;
    }

    public IReadOnlyList<OrderResult> GetAll() => orders.AsReadOnly();

    public OrderResult? GetById(int id)
    {
        if (id < 0 || id >= orders.Count)
            return null;

        return orders[id];
    }

    public void Clear()
    {
        orders.Clear();
    }
}
