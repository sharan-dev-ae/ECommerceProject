namespace CategoryService.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
    public class CategoryCreateRequest
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
