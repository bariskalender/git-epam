using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data;

public class TradeMarketDbContext : DbContext
{
    public TradeMarketDbContext(DbContextOptions<TradeMarketDbContext> options)
        : base(options)
    {
    }

    public DbSet<Person> Persons => this.Set<Person>();

    public DbSet<Customer> Customers => this.Set<Customer>();

    public DbSet<ProductCategory> ProductCategories => this.Set<ProductCategory>();

    public DbSet<Product> Products => this.Set<Product>();

    public DbSet<Receipt> Receipts => this.Set<Receipt>();

    public DbSet<ReceiptDetail> ReceiptDetails => this.Set<ReceiptDetail>();

    public DbSet<ReceiptDetail> ReceiptsDetails => this.Set<ReceiptDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Person)
            .WithOne()
            .HasForeignKey<Customer>(c => c.PersonId);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.ProductCategoryId);

        modelBuilder.Entity<Receipt>()
            .HasOne(r => r.Customer)
            .WithMany(c => c.Receipts)
            .HasForeignKey(r => r.CustomerId);

        modelBuilder.Entity<ReceiptDetail>()
            .HasOne(rd => rd.Receipt)
            .WithMany(r => r.ReceiptDetails)
            .HasForeignKey(rd => rd.ReceiptId);

        modelBuilder.Entity<ReceiptDetail>()
            .HasOne(rd => rd.Product)
            .WithMany(p => p.ReceiptDetails)
            .HasForeignKey(rd => rd.ProductId);
    }
}
