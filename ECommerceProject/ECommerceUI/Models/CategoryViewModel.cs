namespace ECommerceUI.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class CategoryListViewModel
    {
        public List<Category> Categories { get; set; } = new();
        public Category? EditCategory { get; set; }
    }

}
