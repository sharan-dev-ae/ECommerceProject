using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.CQRS.Queries;
using ProductService.Data;
using ProductService.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService.CQRS.Handlers
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
    {
        private readonly ProductDbContext _context;

        public GetAllProductsQueryHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(p => p.ProductImages)
                .ToListAsync(cancellationToken);
        }
    }
}
