using MediatR;
using ProductOrderApi.Infrastructure.Interfaces;
using System.Data;

namespace ProductOrderApi.Commands.Products
{
    internal class DeleteProductCommand : IRequest, ITransactionDependent
    {
        public int Id { get; private set; }

        public IsolationLevel IsolationLevel => IsolationLevel.Serializable;

        public DeleteProductCommand(int id)
        {
            Id = id;
        }
    }

    internal class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductService _service;

        public DeleteProductCommandHandler(IProductService service)
        {
            _service = service;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _service.GetProductAsync(request.Id);

            await _service.DeleteProductAsync(product);
        }
    }
}
