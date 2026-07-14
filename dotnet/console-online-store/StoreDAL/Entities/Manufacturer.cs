using System.ComponentModel.DataAnnotations.Schema;

namespace StoreDAL.Entities;

[Table("manufacturers")]
public class Manufacturer : BaseEntity
{
    public Manufacturer()
        : base()
    {
    }

    public Manufacturer(int id, string name)
        : base(id)
    {
        this.Name = name;
    }

    [Column("manufacturer_name")]
    public string Name { get; set; } = string.Empty;
}