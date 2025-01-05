using MediatR;
using ProductOrderApi.Infrastructure.Interfaces;
using System.Data;

namespace ProductOrderApi.Commands.Orders
{
    internal class DeleteOrderCommand : IRequest, ITransactionDependent
    {
        public int Id { get; private set; }

        public IsolationLevel IsolationLevel => IsolationLevel.Serializable;

        public DeleteOrderCommand(int id)
        {
            Id = id;
        }
    }

    internal class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderService _service;

        public DeleteOrderCommandHandler(IOrderService service)
        {
            _service = service;
        }

        public async Task Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _service.GetOrderAsync(command.Id);

            await _service.DeleteOrderAsync(order);
        }
    }
}
