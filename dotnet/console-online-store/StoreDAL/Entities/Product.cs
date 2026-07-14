using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreDAL.Entities;

[Table("products")]
public class Product : BaseEntity
{
    public Product()
        : base()
    {
    }

    public Product(int id, int titleId, int manufacturerId, string description, decimal price)
        : base(id)
    {
        this.TitleId = titleId;
        this.ManufacturerId = manufacturerId;
        this.Description = description;
        this.UnitPrice = price;
    }

    [Column("product_title_id")]
    public int TitleId { get; set; }

    [Column("manufacturer_id")]
    public int ManufacturerId { get; set; }

    [Column("unit_price")]
    public decimal UnitPrice { get; set; }

    [Column("comment")]
    public string Description { get; set; } = string.Empty;

    [ForeignKey(nameof(TitleId))]
    public ProductTitle ProductTitle { get; set; } = null!;

    [ForeignKey(nameof(ManufacturerId))]
    public Manufacturer Manufacturer { get; set; } = null!;

    public virtual IList<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}