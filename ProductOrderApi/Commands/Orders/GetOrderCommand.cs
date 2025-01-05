using MediatR;
using ProductOrderApi.Entities;
using ProductOrderApi.Helpers;
using ProductOrderApi.Helpers.Exceptions;
using ProductOrderApi.Infrastructure.Interfaces;

namespace ProductOrderApi.Commands.Orders
{
    internal class GetOrderCommand : IRequest<Order>, IUserDependent
    {
        public int Id { get; private set; }

        public User CurrentUser { get; set; } = null!;

        public GetOrderCommand(int id)
        {
            Id = id;
        }
    }

    internal class GetOrderCommandHandler : IRequestHandler<GetOrderCommand, Order>
    {
        private readonly IOrderService _service;

        public GetOrderCommandHandler(IOrderService service)
        {
            _service = service;
        }

        public async Task<Order> Handle(GetOrderCommand command, CancellationToken cancellationToken)
        {
            var order = await _service.GetOrderAsync(command.Id);

            if (command.CurrentUser.IsInRole("Customer") && order.UserId != command.CurrentUser.Id)
            {
                throw new AccessDeniedApiException();
            }

            return order;
        }
    }
}
