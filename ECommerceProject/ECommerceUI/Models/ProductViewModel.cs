namespace ECommerceUI.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string Manufacturer { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public List<string> ImageUrls { get; set; } = new();
    }
    public class ProductListViewModel
    {
        public List<Product> Products { get; set; }
        public Product? EditProduct { get; set; }
    }



}
