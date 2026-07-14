using Microsoft.EntityFrameworkCore;
using ShopReports.Models;
using ShopReports.Reports;

namespace ShopReports.Services;

public class CustomerReportService : IDisposable
{
    private readonly ShopContext shopContext;

    public CustomerReportService(ShopContext shopContext)
    {
        this.shopContext = shopContext;
    }

    public CustomerSalesRevenueReport GetCustomerSalesRevenueReport()
    {
        var lines = this.shopContext.Orders
            .AsNoTracking()
            .Where(o => o.CustomerId.HasValue)
            .GroupBy(o => new
            {
                CustomerId = o.CustomerId!.Value,
                FirstName = o.Customer!.Person.FirstName,
                LastName = o.Customer.Person.LastName,
            })
            .Select(g => new CustomerSalesRevenueReportLine
            {
                CustomerId = g.Key.CustomerId,
                PersonFirstName = g.Key.FirstName,
                PersonLastName = g.Key.LastName,
                SalesRevenue = g.SelectMany(o => o.Details).Sum(d => d.PriceWithDiscount),
            })
            .OrderByDescending(x => x.SalesRevenue)
            .ThenBy(x => x.CustomerId)
            .ToList();

        return new CustomerSalesRevenueReport(lines, DateTime.Now);
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
