namespace StoreBLL.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        public int TitleId { get; set; }

        public int ManufacturerId { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }
    }
}