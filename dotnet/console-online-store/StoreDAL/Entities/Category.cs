using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreDAL.Entities;

[Table("categories")]
public class Category : BaseEntity
{
    public Category()
        : base()
    {
    }

    public Category(int id, string name)
        : base(id)
    {
        this.Name = name;
    }

    [Column("category_name")]
    public string Name { get; set; } = string.Empty;

    public virtual IList<ProductTitle> Titles { get; set; } = new List<ProductTitle>();
}