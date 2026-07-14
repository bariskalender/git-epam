using System;
using ConsoleApp.Controllers;
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
    public static class UserController
    {
        private static readonly StoreDbContext Context = UserMenuController.Context;

        public static void AddUser()
        {
            throw new NotSupportedException();
        }

        public static void UpdateUser()
        {
            throw new NotSupportedException();
        }

        public static void DeleteUser()
        {
            throw new NotSupportedException();
        }

        public static void ShowUser()
        {
            throw new NotSupportedException();
        }

        public static void ShowAllUsers()
        {
            throw new NotSupportedException();
        }

        public static void AddUserRole()
        {
            throw new NotSupportedException();
        }

        public static void UpdateUserRole()
        {
            throw new NotSupportedException();
        }

        public static void DeleteUserRole()
        {
            throw new NotSupportedException();
        }

        public static void ShowAllUserRoles()
        {
            var service = new UserRoleService(Context);

            var menu = new ContextMenu(
                new AdminContextMenuHandler(service, InputHelper.ReadUserRoleModel),
                service.GetAll);

            menu.Run();
        }

        public static void AddProductTitle()
        {
            throw new NotSupportedException();
        }

        public static void UpdateProductTitle()
        {
            throw new NotSupportedException();
        }

        public static void DeleteProductTitle()
        {
            throw new NotSupportedException();
        }

        public static void ShowAllProductTitles()
        {
            throw new NotSupportedException();
        }

        public static void AddManufacturer()
        {
            throw new NotSupportedException();
        }

        public static void UpdateManufacturer()
        {
            throw new NotSupportedException();
        }

        public static void DeleteManufacturer()
        {
            throw new NotSupportedException();
        }

        public static void ShowAllManufacturers()
        {
            throw new NotSupportedException();
        }

        public static void Register()
        {
            Console.WriteLine("Name:");
            var name = Console.ReadLine();

            Console.WriteLine("Last Name:");
            var lastName = Console.ReadLine();

            Console.WriteLine("Login:");
            var login = Console.ReadLine();

            Console.WriteLine("Password:");
            var password = Console.ReadLine();

            var userService = new UserService(new UserRepository(Context));

            var user = new UserModel(0, name ?? string.Empty)
            {
                LastName = lastName ?? string.Empty,
                Login = login ?? string.Empty,
                Password = password ?? string.Empty,
                RoleId = 2,
            };

            userService.Register(user);

            Console.WriteLine("User registered successfully!");
            Console.ReadKey();
        }
    }
}