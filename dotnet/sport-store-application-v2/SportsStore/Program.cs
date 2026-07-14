using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using SportsStore.Models.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StoreDbContext>(opts =>
{
    opts.UseSqlite(
        builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
});

builder.Services.AddScoped<IStoreRepository, EfStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();

// SESSION
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<Cart>(SessionCart.GetCart);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

app.UseStaticFiles();

// SESSION
app.UseSession();

app.MapControllerRoute(
    name: "pagination",
    pattern: "Products/Page{productPage:int}",
    defaults: new
    {
        Controller = "Home",
        action = "Index",
        productPage = 1
    });

app.MapControllerRoute(
    name: "categoryPage",
    pattern: "{category}/Page{productPage:int}",
    defaults: new
    {
        Controller = "Home",
        action = "Index"
    });

app.MapControllerRoute(
    name: "category",
    pattern: "Products/{category}",
    defaults: new
    {
        Controller = "Home",
        action = "Index",
        productPage = 1
    });

app.MapControllerRoute(
    name: "shoppingCart",
    pattern: "Cart",
    defaults: new
    {
        Controller = "Cart",
        action = "Index"
    });

app.MapControllerRoute(
    name: "default",
    pattern: "/",
    defaults: new
    {
        Controller = "Home",
        action = "Index"
    });

app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);

app.Run();

public partial class Program
{
    protected Program()
    {
    }
}
