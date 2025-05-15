using MediatR;
using ProductService.Models;
using System.Collections.Generic;

namespace ProductService.CQRS.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
    {
    }
}
