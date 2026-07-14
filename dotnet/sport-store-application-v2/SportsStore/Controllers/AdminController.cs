using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.Repository;

namespace SportsStore.Controllers;

[Route("Admin")]
public class AdminController : Controller
{
    private readonly IStoreRepository storeRepository;

    private readonly IOrderRepository orderRepository;

    public AdminController(
        IStoreRepository storeRepository,
        IOrderRepository orderRepository)
    {
        this.storeRepository = storeRepository;
        this.orderRepository = orderRepository;
    }

    [Route("Orders")]
    public ViewResult Orders()
    {
        return View(orderRepository.Orders);
    }

    [Route("Products")]
    public ViewResult Products()
    {
        return View(storeRepository.Products);
    }

    [HttpPost]
    [Route("MarkShipped")]
    public IActionResult MarkShipped(int orderId)
    {
        Order? order = orderRepository.Orders
            .FirstOrDefault(o => o.OrderId == orderId);

        if (order != null)
        {
            order.Shipped = true;
            orderRepository.SaveOrder(order);
        }

        return RedirectToAction(nameof(Orders));
    }

    [HttpPost]
    [Route("Reset")]
    public IActionResult Reset(int orderId)
    {
        Order? order = orderRepository.Orders
            .FirstOrDefault(o => o.OrderId == orderId);

        if (order != null)
        {
            order.Shipped = false;
            orderRepository.SaveOrder(order);
        }

        return RedirectToAction(nameof(Orders));
    }

    [Route("Details/{productId:int}")]
    public ViewResult Details(int productId)
    {
        return View(
            storeRepository.Products
                .FirstOrDefault(p => p.ProductId == productId));
    }

    [Route("Products/Edit/{productId:long}")]
    public ViewResult Edit(long productId)
    {
        return View(GetProduct(productId));
    }

    [HttpPost]
    [Route("Products/Edit")]
    public IActionResult Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            storeRepository.SaveProduct(product);

            return RedirectToAction(nameof(Products));
        }

        return View(product);
    }

    [Route("Products/Create")]
    public ViewResult Create()
    {
        return View("Edit", new Product());
    }

    [HttpPost]
    [Route("Products/Create")]
    public IActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            storeRepository.SaveProduct(product);

            return RedirectToAction(nameof(Products));
        }

        return View("Edit", product);
    }

    [Route("Products/Delete/{productId:long}")]
    public IActionResult Delete([FromRoute] long productId)
    {
        return View(GetProduct(productId));
    }

    [HttpPost]
    [Route("Products/Delete/{productId:long}")]
    public IActionResult DeleteProduct(long productId)
    {
        Product? product = GetProduct(productId);

        if (product != null)
        {
            storeRepository.DeleteProduct(product);
        }

        return RedirectToAction(nameof(Products));
    }

    private Product? GetProduct(long productId)
    {
        return storeRepository.Products
            .FirstOrDefault(p => p.ProductId == productId);
    }
}
