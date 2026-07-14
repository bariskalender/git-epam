using System;
using System.Collections.Generic;
using ConsoleMenu;
using ConsoleMenu.Builder;
using StoreBLL.Services;
using StoreDAL.Data;
using StoreDAL.Data.InitDataFactory;
using StoreDAL.Repository;

namespace ConsoleApp1
{
    public enum UserRoles
    {
        Guest,
        Administrator,
        RegistredCustomer,
    }

    public static class UserMenuController
    {
        private static readonly Dictionary<UserRoles, Menu> RolesToMenu;
        private static readonly StoreDbContext ContextField;
        private static UserRoles userRole;

        static UserMenuController()
        {
            userRole = UserRoles.Guest;
            RolesToMenu = new Dictionary<UserRoles, Menu>();

            var factory = new StoreDbFactory(new TestDataFactory());
            ContextField = factory.CreateContext();

            RolesToMenu.Add(UserRoles.Guest, new GuestMainMenu().Create(ContextField));
            RolesToMenu.Add(UserRoles.RegistredCustomer, new UserMainMenu().Create(ContextField));
            RolesToMenu.Add(UserRoles.Administrator, new AdminMainMenu().Create(ContextField));
        }

        public static StoreDbContext Context
        {
            get
            {
                return ContextField;
            }
        }

        public static void Login()
        {
            Console.WriteLine("Login:");
            var login = Console.ReadLine();

            Console.WriteLine("Password:");
            var password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Invalid login or password");
                Console.ReadKey();
                return;
            }

            var userService = new UserService(new UserRepository(ContextField));
            var user = userService.Login(login, password);

            if (user == null)
            {
                Console.WriteLine("Invalid login or password");
                Console.ReadKey();
                return;
            }

            if (user.RoleId == 1)
            {
                userRole = UserRoles.Administrator;
            }
            else if (user.RoleId == 2)
            {
                userRole = UserRoles.RegistredCustomer;
            }
            else
            {
                userRole = UserRoles.Guest;
            }
        }

        public static void Logout()
        {
            userRole = UserRoles.Guest;
        }

        public static void Start()
        {
            ConsoleKey resKey;
            var updateItems = true;

            do
            {
                resKey = RolesToMenu[userRole].RunOnce(ref updateItems);
            }
            while (resKey != ConsoleKey.Escape);
        }
    }
}