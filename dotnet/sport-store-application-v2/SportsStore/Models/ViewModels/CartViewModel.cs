#pragma warning disable CA1056

using SportsStore.Models;

namespace SportsStore.Models.ViewModels;

public class CartViewModel
{
    public Cart Cart { get; set; } = new();

    public string ReturnUrl { get; set; } = string.Empty;
}

#pragma warning restore CA1056
