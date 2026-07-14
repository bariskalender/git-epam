using Microsoft.AspNetCore.Mvc;
using Northwind.Orders.WebApi.Models;
using Northwind.Services.Repositories;
using ModelsCustomer = Northwind.Orders.WebApi.Models.Customer;
using ModelsEmployee = Northwind.Orders.WebApi.Models.Employee;
using ModelsShipper = Northwind.Orders.WebApi.Models.Shipper;
using ModelsShippingAddress = Northwind.Orders.WebApi.Models.ShippingAddress;
using RepositoryCustomer = Northwind.Services.Repositories.Customer;
using RepositoryCustomerCode = Northwind.Services.Repositories.CustomerCode;
using RepositoryEmployee = Northwind.Services.Repositories.Employee;
using RepositoryOrder = Northwind.Services.Repositories.Order;
using RepositoryOrderDetail = Northwind.Services.Repositories.OrderDetail;
using RepositoryProduct = Northwind.Services.Repositories.Product;
using RepositoryShipper = Northwind.Services.Repositories.Shipper;
using RepositoryShippingAddress = Northwind.Services.Repositories.ShippingAddress;

namespace Northwind.Orders.WebApi.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly IOrderRepository orderRepository;
    private readonly ILogger<OrdersController> logger;

    public OrdersController(IOrderRepository orderRepository, ILogger<OrdersController> logger)
    {
        this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{orderId:long}")]
    public async Task<ActionResult<FullOrder>> GetOrderAsync(long orderId)
    {
        try
        {
            RepositoryOrder order = await this.orderRepository.GetOrderAsync(orderId);
            return this.Ok(MapFullOrder(order));
        }
        catch (OrderNotFoundException ex)
        {
            this.logger.LogError(ex, "Order {OrderId} was not found.", orderId);
            return this.NotFound();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unhandled error while getting order {OrderId}.", orderId);
            return this.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BriefOrder>>> GetOrdersAsync(int? skip, int? count)
    {
        int actualSkip = skip ?? 0;
        int actualCount = count ?? 10;

        if (actualSkip < 0 || actualCount <= 0)
        {
            this.logger.LogError("Invalid paging arguments. Skip: {Skip}, Count: {Count}.", actualSkip, actualCount);
            return this.BadRequest();
        }

        try
        {
            IList<RepositoryOrder> orders = await this.orderRepository.GetOrdersAsync(actualSkip, actualCount);
            return this.Ok(orders.Select(MapBriefOrder).ToList());
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unhandled error while getting orders.");
            return this.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    public Task<ActionResult<AddOrder>> AddOrderAsync(BriefOrder order)
    {
        ArgumentNullException.ThrowIfNull(order);

        return this.AddOrderCoreAsync(order);
    }

    [HttpDelete("{orderId:long}")]
    public async Task<ActionResult> RemoveOrderAsync(long orderId)
    {
        try
        {
            await this.orderRepository.RemoveOrderAsync(orderId);
            return this.NoContent();
        }
        catch (OrderNotFoundException ex)
        {
            this.logger.LogError(ex, "Order {OrderId} was not found.", orderId);
            return this.NotFound();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unhandled error while removing order {OrderId}.", orderId);
            return this.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{orderId:long}")]
    public Task<ActionResult> UpdateOrderAsync(long orderId, BriefOrder order)
    {
        ArgumentNullException.ThrowIfNull(order);

        return this.UpdateOrderCoreAsync(orderId, order);
    }

    private static BriefOrder MapBriefOrder(RepositoryOrder order)
    {
        return new BriefOrder
        {
            Id = order.Id,
            CustomerId = order.Customer.Code.Code,
            EmployeeId = order.Employee.Id,
            OrderDate = order.OrderDate,
            RequiredDate = order.RequiredDate,
            ShippedDate = order.ShippedDate,
            ShipperId = order.Shipper.Id,
            Freight = order.Freight,
            ShipName = order.ShipName,
            ShipAddress = order.ShippingAddress.Address,
            ShipCity = order.ShippingAddress.City,
            ShipRegion = order.ShippingAddress.Region,
            ShipPostalCode = order.ShippingAddress.PostalCode,
            ShipCountry = order.ShippingAddress.Country,
            OrderDetails = order.OrderDetails.Select(MapBriefOrderDetail).ToList(),
        };
    }

    private static BriefOrderDetail MapBriefOrderDetail(RepositoryOrderDetail detail)
    {
        return new BriefOrderDetail
        {
            ProductId = detail.Product.Id,
            UnitPrice = detail.UnitPrice,
            Quantity = detail.Quantity,
            Discount = detail.Discount,
        };
    }

    private static FullOrder MapFullOrder(RepositoryOrder order)
    {
        return new FullOrder
        {
            Id = order.Id,
            Customer = new ModelsCustomer
            {
                Code = order.Customer.Code.Code,
                CompanyName = order.Customer.CompanyName,
            },
            Employee = new ModelsEmployee
            {
                Id = order.Employee.Id,
                FirstName = order.Employee.FirstName,
                LastName = order.Employee.LastName,
                Country = order.Employee.Country,
            },
            Shipper = new ModelsShipper
            {
                Id = order.Shipper.Id,
                CompanyName = order.Shipper.CompanyName,
            },
            OrderDate = order.OrderDate,
            RequiredDate = order.RequiredDate,
            ShippedDate = order.ShippedDate,
            Freight = order.Freight,
            ShipName = order.ShipName,
            ShippingAddress = new ModelsShippingAddress
            {
                Address = order.ShippingAddress.Address,
                City = order.ShippingAddress.City,
                Region = order.ShippingAddress.Region,
                PostalCode = order.ShippingAddress.PostalCode,
                Country = order.ShippingAddress.Country,
            },
            OrderDetails = order.OrderDetails.Select(MapFullOrderDetail).ToList(),
        };
    }

    private static FullOrderDetail MapFullOrderDetail(RepositoryOrderDetail detail)
    {
        return new FullOrderDetail
        {
            ProductId = detail.Product.Id,
            ProductName = detail.Product.ProductName,
            CategoryId = detail.Product.CategoryId,
            CategoryName = detail.Product.Category,
            SupplierId = detail.Product.SupplierId,
            SupplierCompanyName = detail.Product.Supplier,
            UnitPrice = detail.UnitPrice,
            Quantity = detail.Quantity,
            Discount = detail.Discount,
        };
    }

    private static RepositoryOrder MapRepositoryOrder(BriefOrder order, long orderId)
    {
        var repositoryOrder = new RepositoryOrder(orderId)
        {
            Customer = new RepositoryCustomer(new RepositoryCustomerCode(order.CustomerId))
            {
                CompanyName = string.Empty,
            },
            Employee = new RepositoryEmployee(order.EmployeeId)
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                Country = string.Empty,
            },
            Shipper = new RepositoryShipper(order.ShipperId)
            {
                CompanyName = string.Empty,
            },
            OrderDate = order.OrderDate,
            RequiredDate = order.RequiredDate,
            ShippedDate = order.ShippedDate,
            Freight = order.Freight,
            ShipName = order.ShipName,
            ShippingAddress = new RepositoryShippingAddress(
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
                Product = new RepositoryProduct(detail.ProductId)
                {
                    ProductName = string.Empty,
                    Supplier = string.Empty,
                    Category = string.Empty,
                },
                UnitPrice = detail.UnitPrice,
                Quantity = detail.Quantity,
                Discount = detail.Discount,
            });
        }

        return repositoryOrder;
    }

    private async Task<ActionResult<AddOrder>> AddOrderCoreAsync(BriefOrder order)
    {
        try
        {
            long orderId = await this.orderRepository.AddOrderAsync(MapRepositoryOrder(order, order.Id));
            return this.Ok(new AddOrder { OrderId = orderId });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unhandled error while adding order.");
            return this.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    private async Task<ActionResult> UpdateOrderCoreAsync(long orderId, BriefOrder order)
    {
        try
        {
            await this.orderRepository.UpdateOrderAsync(MapRepositoryOrder(order, orderId));
            return this.NoContent();
        }
        catch (OrderNotFoundException ex)
        {
            this.logger.LogError(ex, "Order {OrderId} was not found.", orderId);
            return this.NotFound();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unhandled error while updating order {OrderId}.", orderId);
            return this.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
