using MediatR;
using ProductOrderApi.DTOs.Products;
using ProductOrderApi.Infrastructure.Interfaces;
using System.Data;

namespace ProductOrderApi.Commands.Products
{
    internal class UpdateProductCommand : IRequest, ITransactionDependent
    {
        public int Id { get; private set; }
        public UpdateProductRequest Request { get; private set; }

        public IsolationLevel IsolationLevel => IsolationLevel.Serializable;

        public UpdateProductCommand(int id, UpdateProductRequest request)
        {
            Id = id;
            Request = request;
        }
    }

    internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductService _service;

        public UpdateProductCommandHandler(IProductService service)
        {
            _service = service;
        }

        public async Task Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var product = await _service.GetProductAsync(command.Id);

            bool changed = false;

            if (request.Features != null)
            {
                var (toAdd, toUpdate, toDelete) = DetermineCartDifferences(
                    product.Features.ToDictionary(e => e.Feature.Name, e => e.Value),
                    request.Features
                );

                if (toUpdate.Count > 0 || toAdd.Count > 0 || toDelete.Count > 0)
                {
                    changed = true;

                    await _service.UpdateFeaturesAsync(product, toUpdate);
                    await _service.AddFeaturesAsync(product, toAdd);
                    await _service.DeleteFeaturesAsync(product, toDelete);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                product.Name = request.Name;
                changed = true;
            }

            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                product.Code = request.Code;
                changed = true;
            }

            if (request.Description != null)
            {
                product.Description = request.Description;
                changed = true;
            }

            if (request.Price != null)
            {
                product.Price = (decimal)request.Price;
                changed = true;
            }

            if (request.QuantityInStock != null)
            {
                product.QuantityInStock = (int)request.QuantityInStock;
                changed = true;
            }

            if (changed)
            {
                product.ModifiedAt = DateTime.Now;

                await _service.UpdateProductAsync(product);
            }
        }

        private (IDictionary<string, string> toAdd, IDictionary<string, string> toUpdate, IList<string> toDelete) DetermineCartDifferences(IDictionary<string, string> oldValue, IDictionary<string, string> newValue)
        {
            var toUpdate = newValue
                .Where(kv => oldValue.ContainsKey(kv.Key))
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            var toAdd = newValue
                .Where(kv => !oldValue.ContainsKey(kv.Key))
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            var toDelete = oldValue
                .Where(kv => !newValue.ContainsKey(kv.Key))
                .Select(kv => kv.Key)
                .ToList();

            return (toAdd, toUpdate, toDelete);
        }
    }
}