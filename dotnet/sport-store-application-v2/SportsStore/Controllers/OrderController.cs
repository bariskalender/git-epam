using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.Repository;

namespace SportsStore.Controllers;

public class OrderController : Controller
{
    private readonly IOrderRepository repository;

    private readonly Cart cart;

    public OrderController(
        IOrderRepository repository,
        Cart cart)
    {
        this.repository = repository
            ?? throw new ArgumentNullException(nameof(repository));

        this.cart = cart
            ?? throw new ArgumentNullException(nameof(cart));
    }

    [HttpGet]
    public ViewResult Checkout()
    {
        return View(new Order());
    }

    [HttpPost]
    public IActionResult Checkout(Order order)
    {
        if (cart.Lines.Count == 0)
        {
            ModelState.AddModelError(
                string.Empty,
                "Sorry, your cart is empty!");
        }

        if (ModelState.IsValid)
        {
            order.SetLines(cart.Lines);

            repository.SaveOrder(order);

            cart.Clear();

            return View(nameof(Completed), order.OrderId);
        }

        return View(order);
    }

    public ViewResult Completed()
    {
        return View();
    }
}
