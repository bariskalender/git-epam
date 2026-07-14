using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace SportsStore.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services
                .GetRequiredService<IHttpContextAccessor>()
                .HttpContext?
                .Session;

            SessionCart cart = new()
            {
                Session = session
            };

            string? cartData = session?.GetString("Cart");

            if (!string.IsNullOrEmpty(cartData))
            {
                List<CartLine>? deserializedLines =
                    JsonSerializer.Deserialize<List<CartLine>>(cartData);

                if (deserializedLines != null)
                {
                    cart.LoadLines(deserializedLines);
                }
            }

            return cart;
        }

        [JsonIgnore]
        public ISession? Session { get; set; }

        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            SaveCart();
        }

        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            SaveCart();
        }

        public override void Clear()
        {
            base.Clear();
            Session?.Remove("Cart");
        }

        private void SaveCart()
        {
            if (Session != null)
            {
                string cartData = JsonSerializer.Serialize(Lines);
                Session.SetString("Cart", cartData);
            }
        }

        private void LoadLines(List<CartLine> lines)
        {
            Lines.Clear();

            foreach (CartLine line in lines)
            {
                Lines.Add(line);
            }
        }
    }
}
