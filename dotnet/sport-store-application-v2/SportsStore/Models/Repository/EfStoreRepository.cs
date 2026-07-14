namespace SportsStore.Models.Repository;

public class EfStoreRepository : IStoreRepository
{
    private readonly StoreDbContext context;

    public EfStoreRepository(StoreDbContext ctx)
    {
        context = ctx;
    }

    public IQueryable<Product> Products => context.Products;

    public void CreateProduct(Product p)
    {
        context.Products.Add(p);
        context.SaveChanges();
    }

    public void DeleteProduct(Product p)
    {
        context.Products.Remove(p);
        context.SaveChanges();
    }

    public void SaveProduct(Product p)
    {
        if (p.ProductId == 0)
        {
            context.Products.Add(p);
        }
        else
        {
            Product? dbEntry = context.Products
                .FirstOrDefault(product => product.ProductId == p.ProductId);

            if (dbEntry != null)
            {
                dbEntry.Name = p.Name;
                dbEntry.Description = p.Description;
                dbEntry.Price = p.Price;
                dbEntry.Category = p.Category;
            }
        }

        context.SaveChanges();
    }
}
