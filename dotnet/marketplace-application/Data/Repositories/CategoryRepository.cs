using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CategoryRepository : Repository<ProductCategory>, ICategoryRepository
{
    public CategoryRepository(TradeMarketDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<ProductCategory>> GetAllWithDetailsAsync()
    {
        return await this.Context.ProductCategories
            .Include(x => x.Products)
            .ToListAsync();
    }

    public async Task<ProductCategory?> GetByIdWithDetailsAsync(int id)
    {
        return await this.Context.ProductCategories
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
