using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models
{
    public class Cart
    {
        private readonly List<CartLine> lineCollection = new();

        public virtual void AddItem(Product product, int quantity)
        {
            CartLine? line = lineCollection
                .FirstOrDefault(
                    p => p.Product.ProductId == product.ProductId);

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product) =>
            lineCollection.RemoveAll(
                l => l.Product.ProductId == product.ProductId);

        public virtual decimal ComputeTotalValue() =>
            lineCollection.Sum(
                e => e.Product.Price * e.Quantity);

        public virtual void Clear() =>
            lineCollection.Clear();

        public virtual List<CartLine> Lines =>
            lineCollection;
    }

    public class CartLine
    {
        public int CartLineID { get; set; }

        public Product Product { get; set; } = new();

        public int Quantity { get; set; }
    }
}
