using MediatR;
using ProductOrderApi.DTOs.Mappers;
using ProductOrderApi.DTOs.Products;
using ProductOrderApi.Entities;
using ProductOrderApi.Infrastructure.Interfaces;
using System.Data;

namespace ProductOrderApi.Commands.Products
{
    internal class CreateProductCommand : IRequest<Product>, ITransactionDependent
    {
        public CreateProductRequest Request { get; private set; }

        public IsolationLevel IsolationLevel => IsolationLevel.ReadCommitted;

        public CreateProductCommand(CreateProductRequest request)
        {
            Request = request;
        }
    }


    internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly IProductService _service;

        public CreateProductCommandHandler(IProductService service)
        {
            _service = service;
        }

        public async Task<Product> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var newProduct = command.Request.ToEntity();

            await _service.AddProductAsync(newProduct);

            if (command.Request.Features != null)
            {
                await _service.AddFeaturesAsync(newProduct, command.Request.Features);
            }

            return newProduct;
        }
    }
}