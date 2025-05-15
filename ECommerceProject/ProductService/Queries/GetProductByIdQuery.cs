using MediatR;
using ProductService.Models;
using System;

namespace ProductService.CQRS.Queries
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public Guid Id { get; set; }
    }
}
