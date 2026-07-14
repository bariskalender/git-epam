using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Models;

public class Order
{
    [BindNever]
    public int OrderId { get; set; }

    private ICollection<CartLine> lines =
        new List<CartLine>();

    [BindNever]
    public ICollection<CartLine> Lines => lines;

    public void SetLines(IEnumerable<CartLine> cartLines)
    {
        lines = cartLines.ToList();
    }

    [Required(ErrorMessage = "Please enter a name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the first address line")]
    public string Line1 { get; set; } = string.Empty;

    public string Line2 { get; set; } = string.Empty;

    public string Line3 { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter a city name")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter a state name")]
    public string State { get; set; } = string.Empty;

    public string Zip { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter a country name")]
    public string Country { get; set; } = string.Empty;

    public bool GiftWrap { get; set; }

    public bool Shipped { get; set; }
}
