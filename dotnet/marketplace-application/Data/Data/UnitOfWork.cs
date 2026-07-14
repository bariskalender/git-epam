using Data.Interfaces;
using Data.Repositories;

namespace Data.Data;

public sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly TradeMarketDbContext context;
    private bool disposed;

    public UnitOfWork(TradeMarketDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;

        this.CustomerRepository = new CustomerRepository(context);
        this.PersonRepository = new PersonRepository(context);
        this.ProductRepository = new ProductRepository(context);
        this.CategoryRepository = new CategoryRepository(context);
        this.ReceiptRepository = new ReceiptRepository(context);
        this.ReceiptDetailRepository = new ReceiptDetailRepository(context);
    }

    public ICustomerRepository CustomerRepository { get; }

    public IPersonRepository PersonRepository { get; }

    public IProductRepository ProductRepository { get; }

    public ICategoryRepository CategoryRepository { get; }

    public IReceiptRepository ReceiptRepository { get; }

    public IReceiptDetailRepository ReceiptDetailRepository { get; }

    public int SaveChanges()
    {
        return this.context.SaveChanges();
    }

    public async Task SaveAsync()
    {
        await this.context.SaveChangesAsync();
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (this.disposed)
        {
            return;
        }

        if (disposing)
        {
            this.context.Dispose();
        }

        this.disposed = true;
    }
}
