using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.CQRS.Commands;
using ProductService.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService.CQRS.Handlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly ProductDbContext _context;

        public DeleteProductCommandHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
