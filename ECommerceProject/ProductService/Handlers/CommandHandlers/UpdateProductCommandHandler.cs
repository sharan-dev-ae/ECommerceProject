using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.CQRS.Commands;
using ProductService.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService.CQRS.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly ProductDbContext _context;

        public UpdateProductCommandHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (product == null) return false;

            product.Name = request.Name;
            product.CategoryName = request.CategoryName;
            product.Manufacturer = request.Manufacturer;
            product.Quantity = request.Quantity;
            product.Price = request.Price;

            _context.Products.Update(product);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
