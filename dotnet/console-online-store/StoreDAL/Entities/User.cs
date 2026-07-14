using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreDAL.Entities;

[Table("users")]
public class User : BaseEntity
{
    public User()
        : base()
    {
    }

    public User(int id, string name, string lastName, string login, string password, int roleId)
        : base(id)
    {
        this.Name = name;
        this.LastName = lastName;
        this.Login = login;
        this.Password = password;
        this.RoleId = roleId;
    }

    [Column("first_name")]
    public string Name { get; set; } = string.Empty;

    [Column("last_name")]
    public string LastName { get; set; } = string.Empty;

    [Column("login")]
    public string Login { get; set; } = string.Empty;

    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("user_role_id")]
    public int RoleId { get; set; }

    [ForeignKey(nameof(RoleId))]
    public UserRole Role { get; set; } = null!;

    public virtual IList<CustomerOrder> Orders { get; set; } = new List<CustomerOrder>();
}