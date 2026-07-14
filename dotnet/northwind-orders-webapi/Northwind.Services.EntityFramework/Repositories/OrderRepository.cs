using Microsoft.EntityFrameworkCore;
using Northwind.Services.EntityFramework.Entities;
using Northwind.Services.Repositories;
using RepositoryCustomer = Northwind.Services.Repositories.Customer;
using RepositoryEmployee = Northwind.Services.Repositories.Employee;
using RepositoryOrder = Northwind.Services.Repositories.Order;
using RepositoryOrderDetail = Northwind.Services.Repositories.OrderDetail;
using RepositoryProduct = Northwind.Services.Repositories.Product;
using RepositoryShipper = Northwind.Services.Repositories.Shipper;

namespace Northwind.Services.EntityFramework.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly NorthwindContext context;

    public OrderRepository(NorthwindContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IList<RepositoryOrder>> GetOrdersAsync(int skip, int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(skip);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);

        var orders = await this.GetOrdersQuery()
            .OrderBy(o => o.OrderId)
            .Skip(skip)
            .Take(count)
            .ToListAsync();

        return orders.Select(this.MapOrder).ToList();
    }

    public async Task<RepositoryOrder> GetOrderAsync(long orderId)
    {
        var order = await this.GetOrdersQuery()
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        return order is null ? throw new OrderNotFoundException() : this.MapOrder(order);
    }

    public Task<long> AddOrderAsync(RepositoryOrder order)
    {
        ArgumentNullException.ThrowIfNull(order);
        ValidateOrderDetails(order);

        return this.AddOrderCoreAsync(order);
    }

    public async Task RemoveOrderAsync(long orderId)
    {
        var order = await this.context.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order is null)
        {
            throw new OrderNotFoundException();
        }

        this.context.OrderDetails.RemoveRange(order.OrderDetails);
        this.context.Orders.Remove(order);

        await this.context.SaveChangesAsync();
    }

    public Task UpdateOrderAsync(RepositoryOrder order)
    {
        ArgumentNullException.ThrowIfNull(order);
        ValidateOrderDetails(order);

        return this.UpdateOrderCoreAsync(order);
    }

    private static void ValidateOrderDetails(RepositoryOrder order)
    {
        foreach (var detail in order.OrderDetails)
        {
            if (detail.Product.Id <= 0 || detail.UnitPrice < 0 || detail.Quantity <= 0 || detail.Discount < 0 || detail.Discount > 1)
            {
                throw new RepositoryException();
            }
        }
    }

    private IQueryable<Entities.Order> GetOrdersQuery()
    {
        return this.context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .Include(o => o.Shipper)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                    .ThenInclude(p => p.Category)
            .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                    .ThenInclude(p => p.Supplier);
    }

    private async Task<long> AddOrderCoreAsync(RepositoryOrder order)
    {
        try
        {
            await using var transaction = await this.context.Database.BeginTransactionAsync();

            var entity = new Entities.Order
            {
                CustomerId = order.Customer.Code.Code,
                EmployeeId = order.Employee.Id,
                OrderDate = order.OrderDate,
                RequiredDate = order.RequiredDate,
                ShippedDate = order.ShippedDate,
                ShipVia = order.Shipper.Id,
                Freight = order.Freight,
                ShipName = order.ShipName,
                ShipAddress = order.ShippingAddress.Address,
                ShipCity = order.ShippingAddress.City,
                ShipRegion = order.ShippingAddress.Region,
                ShipPostalCode = order.ShippingAddress.PostalCode,
                ShipCountry = order.ShippingAddress.Country,
            };

            await this.context.Orders.AddAsync(entity);
            await this.context.SaveChangesAsync();

            foreach (var detail in order.OrderDetails)
            {
                await this.context.OrderDetails.AddAsync(new Entities.OrderDetail
                {
                    OrderId = entity.OrderId,
                    ProductId = detail.Product.Id,
                    UnitPrice = detail.UnitPrice,
                    Quantity = detail.Quantity,
                    Discount = detail.Discount,
                });
            }

            await this.context.SaveChangesAsync();
            await transaction.CommitAsync();

            return entity.OrderId;
        }
        catch (RepositoryException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepositoryException(ex.Message, ex);
        }
    }

    private async Task UpdateOrderCoreAsync(RepositoryOrder order)
    {
        var existingOrder = await this.context.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.OrderId == order.Id);

        if (existingOrder is null)
        {
            throw new OrderNotFoundException();
        }

        existingOrder.CustomerId = order.Customer.Code.Code;
        existingOrder.EmployeeId = order.Employee.Id;
        existingOrder.OrderDate = order.OrderDate;
        existingOrder.RequiredDate = order.RequiredDate;
        existingOrder.ShippedDate = order.ShippedDate;
        existingOrder.ShipVia = order.Shipper.Id;
        existingOrder.Freight = order.Freight;
        existingOrder.ShipName = order.ShipName;
        existingOrder.ShipAddress = order.ShippingAddress.Address;
        existingOrder.ShipCity = order.ShippingAddress.City;
        existingOrder.ShipRegion = order.ShippingAddress.Region;
        existingOrder.ShipPostalCode = order.ShippingAddress.PostalCode;
        existingOrder.ShipCountry = order.ShippingAddress.Country;

        this.context.OrderDetails.RemoveRange(existingOrder.OrderDetails);

        foreach (var detail in order.OrderDetails)
        {
            await this.context.OrderDetails.AddAsync(new Entities.OrderDetail
            {
                OrderId = existingOrder.OrderId,
                ProductId = detail.Product.Id,
                UnitPrice = detail.UnitPrice,
                Quantity = detail.Quantity,
                Discount = detail.Discount,
            });
        }

        await this.context.SaveChangesAsync();
    }

    private RepositoryOrder MapOrder(Entities.Order order)
    {
        var repositoryOrder = new RepositoryOrder(order.OrderId)
        {
            Customer = new RepositoryCustomer(new CustomerCode(order.Customer.CustomerId))
            {
                CompanyName = order.Customer.CompanyName,
            },
            Employee = new RepositoryEmployee(order.Employee.EmployeeId)
            {
                FirstName = order.Employee.FirstName,
                LastName = order.Employee.LastName,
                Country = order.Employee.Country ?? string.Empty,
            },
            Shipper = new RepositoryShipper(order.Shipper.ShipperId)
            {
                CompanyName = order.Shipper.CompanyName,
            },
            OrderDate = order.OrderDate,
            RequiredDate = order.RequiredDate,
            ShippedDate = order.ShippedDate,
            Freight = order.Freight,
            ShipName = order.ShipName,
            ShippingAddress = new ShippingAddress(
                order.ShipAddress,
                order.ShipCity,
                order.ShipRegion,
                order.ShipPostalCode,
                order.ShipCountry),
        };

        foreach (var detail in order.OrderDetails)
        {
            repositoryOrder.OrderDetails.Add(new RepositoryOrderDetail(repositoryOrder)
            {
                Product = new RepositoryProduct(detail.Product.ProductId)
                {
                    ProductName = detail.Product.ProductName,
                    CategoryId = detail.Product.CategoryId,
                    Category = detail.Product.Category.CategoryName,
                    SupplierId = detail.Product.SupplierId,
                    Supplier = detail.Product.Supplier.CompanyName,
                },
                UnitPrice = detail.UnitPrice,
                Quantity = detail.Quantity,
                Discount = detail.Discount,
            });
        }

        return repositoryOrder;
    }
}
