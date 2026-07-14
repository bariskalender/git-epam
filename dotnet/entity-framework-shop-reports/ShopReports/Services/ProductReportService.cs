using Microsoft.EntityFrameworkCore;
using ShopReports.Models;
using ShopReports.Reports;

namespace ShopReports.Services;

public class ProductReportService : IDisposable
{
    private readonly ShopContext shopContext;

    public ProductReportService(ShopContext shopContext)
    {
        this.shopContext = shopContext;
    }

    public ProductCategoryReport GetProductCategoryReport()
    {
        var lines = this.shopContext.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ThenBy(c => c.Id)
            .Select(c => new ProductCategoryReportLine
            {
                CategoryId = c.Id,
                CategoryName = c.Name,
            })
            .ToList();

        return new ProductCategoryReport(lines, DateTime.Now);
    }



    public ProductReport GetProductReport()
    {
        var lines = this.shopContext.Products
            .AsNoTracking()
            .OrderByDescending(p => p.Title.Title)
            .ThenBy(p => p.TitleId)
            .Select(p => new ProductReportLine
            {
                ProductId = p.TitleId,
                ProductTitle = p.Title.Title,
                Manufacturer = p.Manufacturer.Name,
                Price = p.UnitPrice,
            })
            .ToList();

        return new ProductReport(lines, DateTime.Now);
    }

    public FullProductReport GetFullProductReport()
    {
        var lines = this.shopContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Title.Title)
            .ThenBy(p => p.TitleId)
            .Select(p => new FullProductReportLine
            {
                ProductId = p.TitleId,
                Name = p.Title.Title,
                CategoryId = p.Title.CategoryId,
                Category = p.Title.Category.Name,
                Manufacturer = p.Manufacturer.Name,
                Price = p.UnitPrice,
            })
            .ToList();

        return new FullProductReport(lines, DateTime.Now);
    }

    public ProductTitleSalesRevenueReport GetProductTitleSalesRevenueReport()
    {
        var lines = this.shopContext.OrderDetails
            .AsNoTracking()
            .GroupBy(od => new
            {
                od.Product.TitleId,
                od.Product.Title.Title,
            })
            .Select(g => new ProductTitleSalesRevenueReportLine
            {
                ProductTitleName = g.Key.Title,
                SalesRevenue = g.Sum(x => x.PriceWithDiscount),
                SalesAmount = g.Sum(x => x.ProductAmount),
            })
            .OrderByDescending(x => x.SalesRevenue)
            .ThenBy(x => x.ProductTitleName)
            .ToList();

        return new ProductTitleSalesRevenueReport(lines, DateTime.Now);
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.shopContext.Dispose();
        }
    }
}
