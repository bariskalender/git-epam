using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreDAL.Entities;

[Table("user_roles")]
public class UserRole : BaseEntity
{
    public UserRole()
        : base()
    {
    }

    public UserRole(int id, string roleName)
        : base(id)
    {
        this.RoleName = roleName;
    }

    [Column("user_role_name")]
    public string RoleName { get; set; } = string.Empty;

    public virtual IList<User> Users { get; set; } = new List<User>();
}