using System;
using System.Globalization;
using StoreBLL.Interfaces;
using StoreBLL.Models;

namespace ConsoleApp.Handlers.ContextMenuHandlers
{
    public class AdminContextMenuHandler : ContextMenuHandler
    {
        public AdminContextMenuHandler(ICrud service, Func<AbstractModel> readModel)
            : base(service, readModel)
        {
        }

        public void AddItem()
        {
            this.service.Add(this.readModel());
        }

        public void RemoveItem()
        {
            Console.WriteLine("Input record ID that will be removed");

            if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
            {
                Console.WriteLine("Invalid ID");
                return;
            }

            this.service.Delete(id);
        }

        public void EditItem()
        {
            Console.WriteLine("Input record ID that will be edited");

            if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
            {
                Console.WriteLine("Invalid ID");
                return;
            }

            var record = this.readModel();
            record.Id = id;

            this.service.Update(record);
        }

        public override (ConsoleKey id, string caption, Action action)[] GenerateMenuItems()
        {
            return new (ConsoleKey id, string caption, Action action)[]
            {
                (ConsoleKey.A, "Add Item", this.AddItem),
                (ConsoleKey.R, "Remove Item", this.RemoveItem),
                (ConsoleKey.E, "Edit Item", this.EditItem),
                (ConsoleKey.V, "View Details", this.GetItemDetails),
            };
        }
    }
}