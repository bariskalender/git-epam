using System;
using ConsoleApp1;
using StoreBLL.Services;
using StoreDAL.Data;
using StoreDAL.Repository;

namespace ConsoleApp.Controllers
{
    public static class ProductController
    {
        private static StoreDbContext context = UserMenuController.Context;

        public static void AddProduct()
        {
            throw new NotImplementedException();
        }

        public static void UpdateProduct()
        {
            throw new NotImplementedException();
        }

        public static void DeleteProduct()
        {
            throw new NotImplementedException();
        }

        public static void ShowProduct()
        {
            throw new NotImplementedException();
        }

        public static void ShowAllProducts()
        {
            var productService = new ProductService(
                new ProductRepository(context));

            var products = productService.GetAllProducts();

            foreach (var product in products)
            {
                Console.WriteLine($"Id: {product.Id} | Price: {product.UnitPrice} | Description: {product.Description}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void AddCategory()
        {
            throw new NotImplementedException();
        }

        public static void UpdateCategory()
        {
            throw new NotImplementedException();
        }

        public static void DeleteCategory()
        {
            throw new NotImplementedException();
        }

        public static void ShowAllCategories()
        {
            throw new NotImplementedException();
        }

        public static void AddProductTitle()
        {
            throw new NotImplementedException();
        }

        public static void UpdateProductTitle()
        {
            throw new NotImplementedException();
        }

        public static void DeleteProductTitle()
        {
            throw new NotImplementedException();
        }

        public static void ShowAllProductTitles()
        {
            throw new NotImplementedException();
        }

        public static void AddManufacturer()
        {
            throw new NotImplementedException();
        }

        public static void UpdateManufacturer()
        {
            throw new NotImplementedException();
        }

        public static void DeleteManufacturer()
        {
            throw new NotImplementedException();
        }

        public static void ShowAllManufacturers()
        {
            throw new NotImplementedException();
        }
    }
}