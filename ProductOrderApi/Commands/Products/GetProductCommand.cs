using MediatR;
using ProductOrderApi.Entities;
using ProductOrderApi.Infrastructure.Interfaces;

namespace ProductOrderApi.Commands.Products
{
    internal class GetProductCommand : IRequest<Product>
    {
        public int Id { get; private set; }

        public GetProductCommand(int id)
        {
            Id = id;
        }
    }

    internal class GetProductCommandHandler : IRequestHandler<GetProductCommand, Product>
    {
        private readonly IProductService _service;

        public GetProductCommandHandler(IProductService service)
        {
            _service = service;
        }

        public async Task<Product> Handle(GetProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _service.GetProductAsync(request.Id);

            return product;
        }
    }
}
