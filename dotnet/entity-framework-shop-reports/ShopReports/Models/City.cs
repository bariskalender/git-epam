using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopReports.Models;

[Table("location_city")]
public class City
{
    [Key]
    [Column("city_id")]
    public int Id { get; set; }

    [Column("city")]
    public string Name { get; set; } = null!;

    [Column("country")]
    public string Country { get; set; } = null!;

    public virtual IList<Location> Locations { get; set; } = new List<Location>();
}
