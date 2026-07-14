using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models.Repository;

public class EfOrderRepository : IOrderRepository
{
    private readonly StoreDbContext context;

    public EfOrderRepository(StoreDbContext context)
    {
        this.context = context
            ?? throw new ArgumentNullException(nameof(context));
    }

    public IQueryable<Order> Orders =>
        context.Orders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product);

    public void SaveOrder(Order order)
    {
        context.AttachRange(order.Lines.Select(l => l.Product));

        if (order.OrderId == 0)
        {
            context.Orders.Add(order);
        }

        context.SaveChanges();
    }
}
