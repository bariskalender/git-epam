namespace StoreBLL.Models
{
    public class UserModel : AbstractModel
    {
        public UserModel(int id, string name)
            : base(id)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

        public override string ToString()
        {
            return $"Id: {this.Id} | {this.Name} {this.LastName} | Login: {this.Login} | Role: {this.RoleId}";
        }
    }
}