using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class Repository<T> : IRepository<T>
    where T : BaseEntity
{
    private readonly TradeMarketDbContext context;

    private readonly DbSet<T> dbSet;

    public Repository(TradeMarketDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
        this.dbSet = context.Set<T>();
    }

    protected TradeMarketDbContext Context => this.context;

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await this.dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await this.dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await this.dbSet.AddAsync(entity);
        await this.context.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        this.dbSet.Update(entity);
        this.context.SaveChanges();
    }

    public void Delete(T entity)
    {
        this.dbSet.Remove(entity);
        this.context.SaveChanges();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var entity = await this.GetByIdAsync(id);

        if (entity != null)
        {
            this.dbSet.Remove(entity);
            await this.context.SaveChangesAsync();
        }
    }
}
