namespace StoreBLL.Models
{
    public class ManufacturerModel : AbstractModel
    {
        public ManufacturerModel(int id, string name)
            : base(id)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"Id: {this.Id} | Name: {this.Name}";
        }
    }
}