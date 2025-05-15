using MediatR;
using System;

namespace ProductService.CQRS.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
