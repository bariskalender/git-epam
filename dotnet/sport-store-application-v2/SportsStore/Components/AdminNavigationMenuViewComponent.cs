using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Components;

public class AdminNavigationMenuViewComponent : ViewComponent
{
    private static readonly string[] Items =
    [
        "Orders",
        "Products"
    ];

    public IViewComponentResult Invoke()
    {
        ViewBag.Selection = Request.Path.Value ?? "Products";

        return View(Items);
    }
}
