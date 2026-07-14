using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models;

public class StoreDbContext(DbContextOptions<StoreDbContext> opts)
    : DbContext(opts)
{
    public DbSet<Product> Products => Set<Product>();

    public DbSet<Order> Orders => Set<Order>();
}
