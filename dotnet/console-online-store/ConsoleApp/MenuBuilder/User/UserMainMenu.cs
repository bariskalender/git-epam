using ConsoleApp.Controllers;
using ConsoleApp.Services;
using ConsoleApp1;
using StoreDAL.Data;

namespace ConsoleMenu.Builder;

public class UserMainMenu : AbstractMenuCreator
{
    public override (ConsoleKey id, string caption, Action action)[] GetMenuItems(StoreDbContext context)
    {
        (ConsoleKey id, string caption, Action action)[] array =
        {
            (ConsoleKey.F1, "Logout", UserMenuController.Logout),
            (ConsoleKey.F2, "Show product list", ProductController.ShowAllProducts),
            (ConsoleKey.F3, "Show order list", ShopController.ShowAllOrders),
            (ConsoleKey.F4, "Cancel order", ShopController.DeleteOrder),
            (ConsoleKey.F5, "Confirm order delivery", () => { Console.WriteLine("Confirm order delivery"); }),
            (ConsoleKey.F6, "Add order feedback", () => { Console.WriteLine("Add order feedback"); }),
        };
        return array;
    }
}