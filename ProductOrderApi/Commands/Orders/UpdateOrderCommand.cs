using MediatR;
using ProductOrderApi.DTOs.Mappers;
using ProductOrderApi.DTOs.Orders;
using ProductOrderApi.Entities;
using ProductOrderApi.Helpers;
using ProductOrderApi.Helpers.Exceptions;
using ProductOrderApi.Infrastructure.Interfaces;
using System.Data;
using System.Net;

namespace ProductOrderApi.Commands.Orders
{
    internal class UpdateOrderCommand : IRequest, ITransactionDependent
    {
        public int Id { get; private set; }
        public UpdateOrderRequest Request { get; private set; }

        public IsolationLevel IsolationLevel => IsolationLevel.Serializable;

        public UpdateOrderCommand(int id, UpdateOrderRequest request)
        {
            Id = id;
            Request = request;
        }
    }

    internal class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly RabbitMqSender _mq;

        public UpdateOrderCommandHandler(IOrderService orderService, IProductService productService, RabbitMqSender mq)
        {
            _orderService = orderService;
            _productService = productService;
            _mq = mq;
        }

        private (IList<CartItemDTO> toAdd, IList<CartItemDTO> toUpdate, IList<CartItemDTO> toDelete) DetermineCartDifferences(IList<CartItemDTO> oldValue, IList<CartItemDTO> newValue)
        {
            var toUpdate = newValue
                .Where(x => oldValue.Any(e => e.ProductId == x.ProductId && e.Quantity != x.Quantity))
                .Select(x => new CartItemDTO { ProductId = x.ProductId, Quantity = x.Quantity })
                .ToList();

            var toAdd = newValue
                .Where(x => !oldValue.Any(e => e.ProductId == x.ProductId))
                .ToList();

            var toDelete = oldValue
                .Where(x => !newValue.Any(e => e.ProductId == x.ProductId))
                .ToList();

            return (toAdd, toUpdate, toDelete);
        }

        private async Task UpdateCart(Order order, IList<CartItemDTO> newCart)
        {
            if (newCart.Count == 0)
            {
                throw new ServiceLayerApiException("Order must contain at least one product", HttpStatusCode.BadRequest);
            }

            var (toAdd, toUpdate, toDelete) = DetermineCartDifferences(
                order.Cart.Select(e => e.ToCartItemDTO()).ToList(),
                newCart);

            if (toUpdate.Count > 0 || toAdd.Count > 0 || toDelete.Count > 0)
            {
                var currentOrderStatus = _orderService.GetCurrentOrderStatus(order);

                if (currentOrderStatus != OrderStatus.Created)
                {
                    throw new ServiceLayerApiException($"Order cart with id {order.Id} cannot be updated because its status is {OrderStatusLabels.Parse(currentOrderStatus)}.", HttpStatusCode.Conflict);
                }

                await _orderService.UpdateCartItems(order, toUpdate);
                await _orderService.AddCartItems(order, toAdd);
                await _orderService.DeleteCartItems(order, toDelete);
                await _orderService.UpdateOrderTotalPriceAsync(order);
            }
        }

        public async Task Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var order = await _orderService.GetOrderAsync(command.Id);

            if (request.Cart != null)
            {
                await UpdateCart(order, request.Cart);
            }

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                var currentStatus = _orderService.GetCurrentOrderStatus(order);
                var newStatus = OrderStatusLabels.Parse(request.Status);

                await _orderService.UpdateOrderStatusAsync(order, newStatus);

                await _mq.SendMessage(
                    $"Order with id {order.Id} " +
                    $"has changed its status " +
                    $"from {OrderStatusLabels.Parse(currentStatus)} " +
                    $"to {OrderStatusLabels.Parse(newStatus)} " +
                    $"at {order.Tracking[0].UpdatedAt}"
                );
            }

            if (!string.IsNullOrWhiteSpace(request.Comment))
            {
                order.Comment = request.Comment;

                await _orderService.UpdateOrderAsync(order);
            }
        }
    }
}
