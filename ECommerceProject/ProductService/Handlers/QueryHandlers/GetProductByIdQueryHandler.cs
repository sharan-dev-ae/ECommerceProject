using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.CQRS.Queries;
using ProductService.Data;
using ProductService.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService.CQRS.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly ProductDbContext _context;

        public GetProductByIdQueryHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        }
    }
}
