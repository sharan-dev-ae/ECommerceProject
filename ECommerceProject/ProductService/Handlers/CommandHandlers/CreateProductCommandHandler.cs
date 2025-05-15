using MediatR;
using ProductService.Commands;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Handlers.CommandHandlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly ProductDbContext _context;

        public CreateProductCommandHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                CategoryName = request.CategoryName,
                Manufacturer = request.Manufacturer,
                Quantity = request.Quantity,
                Price = request.Price,
                ProductImages = request.ImageUrls.Select(url => new ProductImage
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = url
                }).ToList()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product;
        }
    }
}
