using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;
using SportsStore.Models.Repository;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers;

public class CartController : Controller
{
    private readonly IStoreRepository repository;

    private readonly Cart cart;

    public CartController(IStoreRepository repository)
    {
        this.repository = repository
            ?? throw new ArgumentNullException(nameof(repository));

        cart = new Cart();
    }

    [ActivatorUtilitiesConstructor]
    public CartController(
        IStoreRepository repository,
        Cart cart)
    {
        this.repository = repository
            ?? throw new ArgumentNullException(nameof(repository));

        this.cart = cart
            ?? throw new ArgumentNullException(nameof(cart));
    }

    [HttpGet]
    public IActionResult Index(object? returnUrl)
    {
        return View(new CartViewModel
        {
            Cart = cart,
            ReturnUrl = returnUrl?.ToString() ?? "Home"
        });
    }

    [HttpPost]
    public IActionResult Index(long productId, object? returnUrl)
    {
        Product? product = repository.Products
            .FirstOrDefault(p => p.ProductId == productId);

        if (product != null)
        {
            cart.AddItem(product, 1);

            return View(new CartViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl?.ToString() ?? "Home"
            });
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Remove(long productId, object? returnUrl)
    {
        CartViewModel viewModel = new()
        {
            Cart = cart,
            ReturnUrl = returnUrl?.ToString() ?? "Home"
        };

        Product? product = repository.Products
            .FirstOrDefault(p => p.ProductId == productId);

        if (product != null)
        {
            cart.RemoveLine(product);
        }

        return View("Index", viewModel);
    }
}
