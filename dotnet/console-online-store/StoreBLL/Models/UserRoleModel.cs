namespace StoreBLL.Models
{
    public class UserRoleModel : AbstractModel
    {
        public UserRoleModel(int id, string roleName)
            : base(id)
        {
            this.RoleName = roleName;
        }

        public string RoleName { get; set; }

        public override string ToString()
        {
            return $"Id: {this.Id} | Role: {this.RoleName}";
        }
    }
}