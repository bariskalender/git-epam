using System;
using System.Globalization;
using StoreBLL.Interfaces;
using StoreBLL.Models;

namespace ConsoleApp.Handlers.ContextMenuHandlers
{
    public class OrderContextMenuHandler : ContextMenuHandler
    {
        public OrderContextMenuHandler(ICrud service, Func<AbstractModel> readModel)
            : base(service, readModel)
        {
        }

        public void RemoveItem()
        {
            Console.WriteLine("Input record ID that will be removed");

            if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int id))
            {
                Console.WriteLine("Invalid ID");
                return;
            }

            this.service.Delete(id);
        }

        public void EditItem()
        {
            Console.WriteLine("Input record ID that will be edited");

            if (!int.TryParse(Console.ReadLine(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int id))
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
                (ConsoleKey.V, "View Details", this.GetItemDetails),
                (ConsoleKey.E, "Change order status", this.EditItem),
            };
        }
    }
}