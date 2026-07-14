using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Components;

public class CartSummaryViewComponent : ViewComponent
{
    private readonly Cart cart;

    public CartSummaryViewComponent(Cart cart)
    {
        this.cart = cart
            ?? throw new ArgumentNullException(nameof(cart));
    }

    public IViewComponentResult Invoke()
    {
        return View(cart);
    }
}
