namespace ProductService.Models
{
    public class Product
    {
      
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string CategoryName { get; set; }

            public string Manufacturer { get; set; }

            public int Quantity { get; set; }

            public decimal Price { get; set; }

            public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();


    }
    public class ProductImage
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }

        public string ImageUrl { get; set; } 

        public Product Product { get; set; }
    }
}
