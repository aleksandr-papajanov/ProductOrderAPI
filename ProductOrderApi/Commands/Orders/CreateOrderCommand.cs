using MediatR;
using ProductOrderApi.DTOs.Mappers;
using ProductOrderApi.DTOs.Orders;
using ProductOrderApi.Entities;
using ProductOrderApi.Infrastructure.Interfaces;
using ProductOrderApi.Services;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;

namespace ProductOrderApi.Commands.Orders
{
    internal class CreateOrderCommand : IRequest<Order>, ITransactionDependent, IUserDependent
    {
        public CreateOrderRequest Request { get; private set; }

        public IsolationLevel IsolationLevel => IsolationLevel.Serializable;

        public User CurrentUser { get; set; } = null!;

        public CreateOrderCommand(CreateOrderRequest request)
        {
            Request = request;
        }
    }

    internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public CreateOrderCommandHandler(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        public async Task<Order> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var order = new Order
            {
                Comment = request.Comment,
                UserId = command.CurrentUser.Id
            };

            await _orderService.AddOrderAsync(order);
            await _orderService.AddCartItems(order, request.Cart);
            await _orderService.UpdateOrderTotalPriceAsync(order);

            return order;
        }
    }
}
