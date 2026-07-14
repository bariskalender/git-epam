namespace StoreBLL.Models
{
    public class ProductTitleModel : AbstractModel
    {
        public ProductTitleModel(int id, string name, int categoryId)
            : base(id)
        {
            this.Name = name;
            this.CategoryId = categoryId;
        }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public override string ToString()
        {
            return $"Id: {this.Id} | Name: {this.Name} | CategoryId: {this.CategoryId}";
        }
    }
}