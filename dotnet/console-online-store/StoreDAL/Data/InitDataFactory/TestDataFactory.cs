using System;
using System.Globalization;
using StoreDAL.Entities;

namespace StoreDAL.Data.InitDataFactory;

public class TestDataFactory : AbstractDataFactory
{
    public override Category[] GetCategoryData()
    {
        return new[]
        {
            new Category(1, "fruits"),
            new Category(2, "water"),
            new Category(3, "vegetables"),
            new Category(4, "seafood"),
            new Category(5, "meet"),
            new Category(6, "grocery"),
            new Category(7, "milk food"),
            new Category(8, "smartphones"),
            new Category(9, "laptop"),
            new Category(10, "photocameras"),
            new Category(11, "kitchen accesories"),
            new Category(12, "spices"),
            new Category(13, "Juice"),
            new Category(14, "alcohol drinks"),
        };
    }

    public override Manufacturer[] GetManufacturerData()
    {
        return new[]
        {
            new Manufacturer(1, "Apple"),
            new Manufacturer(2, "Samsung"),
            new Manufacturer(3, "Dell"),
            new Manufacturer(4, "Sony"),
        };
    }

    public override ProductTitle[] GetProductTitleData()
    {
        return new[]
        {
            new ProductTitle(1, "iPhone 14", 8),
            new ProductTitle(2, "Galaxy S23", 8),
            new ProductTitle(3, "Dell XPS 13", 9),
            new ProductTitle(4, "Sony Camera", 10),
        };
    }

    public override Product[] GetProductData()
    {
        return new[]
        {
            new Product(1, 1, 1, "Apple smartphone", 999),
            new Product(2, 2, 2, "Samsung smartphone", 850),
            new Product(3, 3, 3, "Dell laptop", 1200),
            new Product(4, 4, 4, "Sony camera", 700),
        };
    }

    public override User[] GetUserData()
    {
        return new[]
        {
            new User(1, "Admin", "Admin", "admin", "admin123", 1),
            new User(2, "John", "Doe", "user", "user123", 2),
        };
    }

    public override CustomerOrder[] GetCustomerOrderData()
    {
        return new[]
        {
            new CustomerOrder(
                1,
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                2,
                1),
        };
    }

    public override OrderDetail[] GetOrderDetailData()
    {
        return new[]
        {
            new OrderDetail(1, 1, 1, 999, 1),
            new OrderDetail(2, 1, 2, 850, 2),
        };
    }

    public override OrderState[] GetOrderStateData()
    {
        return new[]
        {
            new OrderState(1, "New Order"),
            new OrderState(2, "Cancelled by user"),
            new OrderState(3, "Cancelled by administrator"),
            new OrderState(4, "Confirmed"),
            new OrderState(5, "Moved to delivery company"),
            new OrderState(6, "In delivery"),
            new OrderState(7, "Delivered to client"),
            new OrderState(8, "Delivery confirmed by client"),
        };
    }

    public override UserRole[] GetUserRoleData()
    {
        return new[]
        {
            new UserRole(1, "Admin"),
            new UserRole(2, "Registered"),
            new UserRole(3, "Guest"),
        };
    }
}