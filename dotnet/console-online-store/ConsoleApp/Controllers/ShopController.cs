using System;
using System.Collections.Generic;
using System.Globalization;
using ConsoleApp.Handlers.ContextMenuHandlers;
using ConsoleApp.Helpers;
using ConsoleApp1;
using ConsoleMenu;
using StoreBLL.Models;
using StoreBLL.Services;
using StoreDAL.Data;
using StoreDAL.Repository;

namespace ConsoleApp.Services
{
    public static class ShopController
    {
        private static readonly StoreDbContext Context = UserMenuController.Context;

        public static void AddOrder()
        {
            var service = GetService();

            Console.WriteLine("Enter User Id:");
            var userIdInput = Console.ReadLine();

            Console.WriteLine("Enter Product Id:");
            var productIdInput = Console.ReadLine();

            Console.WriteLine("Enter Amount:");
            var amountInput = Console.ReadLine();

            Console.WriteLine("Enter Price:");
            var priceInput = Console.ReadLine();

            if (!int.TryParse(userIdInput, NumberStyles.Integer, CultureInfo.InvariantCulture, out var userId) ||
                !int.TryParse(productIdInput, NumberStyles.Integer, CultureInfo.InvariantCulture, out var productId) ||
                !int.TryParse(amountInput, NumberStyles.Integer, CultureInfo.InvariantCulture, out var amount) ||
                !decimal.TryParse(priceInput, NumberStyles.Number, CultureInfo.InvariantCulture, out var price))
            {
                Console.WriteLine("Invalid input");
                Console.ReadKey();
                return;
            }

            var order = new CustomerOrderModel
            {
                UserId = userId,
                OperationTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),

                Details = new List<OrderDetailModel>
                {
                    new OrderDetailModel
                    {
                        ProductId = productId,
                        ProductAmount = amount,
                        Price = price,
                    },
                },
            };

            service.CreateOrder(order);

            Console.WriteLine("Order created!");
            Console.ReadKey();
        }

        public static void DeleteOrder()
        {
            var service = GetService();

            Console.WriteLine("Enter Order Id to cancel:");
            var idInput = Console.ReadLine();

            if (!int.TryParse(idInput, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
            {
                Console.WriteLine("Invalid order id");
                Console.ReadKey();
                return;
            }

            service.CancelOrder(id);

            Console.WriteLine("Order cancelled!");
            Console.ReadKey();
        }

        public static void ShowAllOrders()
        {
            var service = GetService();
            var orders = service.GetAllOrders();

            foreach (var order in orders)
            {
                Console.WriteLine($"Order Id: {order.Id} | UserId: {order.UserId} | State: {order.OrderStateId}");

                foreach (var detail in order.Details)
                {
                    Console.WriteLine($"ProductId: {detail.ProductId} | Amount: {detail.ProductAmount} | Price: {detail.Price}");
                }
            }

            Console.ReadKey();
        }

        public static void ShowAllOrderStates()
        {
            var service = new OrderStateService(Context);

            var menu = new ContextMenu(
                new AdminContextMenuHandler(service, InputHelper.ReadOrderStateModel),
                service.GetAll);

            menu.Run();
        }

        private static CustomerOrderService GetService()
        {
            return new CustomerOrderService(
                new CustomerOrderRepository(Context),
                new OrderDetailRepository(Context));
        }
    }
}