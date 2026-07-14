using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopReports.Models;

[Table("customers")]
public class Customer
{
    [Key]
    [Column("customer_id")]
    [ForeignKey(nameof(Person))]
    public int Id { get; set; }

    [Column("card_number")]
    public string CardNumber { get; set; } = null!;

    [Column("discount")]
    public decimal Discount { get; set; }

    public Person Person { get; set; } = null!;

    public virtual IList<Order> Orders { get; set; } = new List<Order>();
}
