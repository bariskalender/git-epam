namespace Business.Models;

public class CategoryModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<int> ProductIds { get; set; } = new List<int>();
}
