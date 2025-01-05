using Microsoft.EntityFrameworkCore;
using ProductOrderApi.DTOs.Mappers;
using ProductOrderApi.DTOs.Orders;
using ProductOrderApi.Entities;
using ProductOrderApi.Helpers.Exceptions;
using ProductOrderApi.Infrastructure.Interfaces;
using System.Net;
using System.Xml.Linq;

namespace ProductOrderApi.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderProduct> _cartItemsRepository;
        private readonly IRepository<OrderTracking> _trackingRepository;
        private readonly IProductService _productService;

        public IQueryable<Order> Orders => _orderRepository.All;

        public OrderService(
            IRepository<Order> orderRepository,
            IRepository<OrderProduct> cartItemsRepository,
            IRepository<OrderTracking> trackingRepository,
            IProductService productService)
        {
            _orderRepository = orderRepository;
            _cartItemsRepository = cartItemsRepository;
            _trackingRepository = trackingRepository;
            _productService = productService;
        }


        public async Task AddOrderAsync(Order order)
        {
            await _orderRepository.AddAsync(order);

            var tracking = new OrderTracking
            {
                OrderId = order.Id,
                Status = OrderStatus.Created,
                UpdatedAt = DateTime.Now
            };

            await _trackingRepository.AddAsync(tracking);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(Order order)
        {
            await _trackingRepository.DeleteRangeAsync(order.Tracking);
            await DeleteCartItems(order, order.Cart.Select(e => e.ToCartItemDTO()).ToList());
            await _orderRepository.DeleteAsync(order);
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            var order = await _orderRepository.All
                .Include(e => e.Cart)
                    .ThenInclude(e => e.Product)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (order == null)
            {
                throw new EntityNotFoundApiException($"Order specified by id '{id}' hasn't been found");
            }

            order.Tracking = await _trackingRepository.All
                .Where(t => t.OrderId == order.Id)
                .OrderByDescending(t => t.UpdatedAt)
                .ToListAsync();

            return order;
        }

        public async Task UpdateOrderTotalPriceAsync(Order order)
        {
            order.TotalPrice = order.Cart.Sum(item => item.Quantity * item.Price);

            await _orderRepository.UpdateAsync(order);
        }

        public OrderStatus GetCurrentOrderStatus(Order order)
        {
            var current = order.Tracking.FirstOrDefault();

            if (current == null)
            {
                throw new EntityNotFoundApiException($"Could't find any tracking information of the order with id {order.Id}");
            }

            return current.Status;
        }

        public async Task UpdateOrderStatusAsync(Order order, OrderStatus newStatus)
        {
            if (GetCurrentOrderStatus(order) == newStatus)
            {
                throw new ServiceLayerApiException($"Current status of order with id {order.Id} is already {OrderStatusLabels.Parse(newStatus)}", HttpStatusCode.Conflict);
            }

            var currentTracking = new OrderTracking
            {
                OrderId = order.Id,
                UpdatedAt = DateTime.Now,
                Status = newStatus,
            };

            await _trackingRepository.AddAsync(currentTracking);
        }

        public async Task AddCartItems(Order order, IList<CartItemDTO> items)
        {
            foreach (var item in items)
            {
                var product = await _productService.GetProductAsync(item.ProductId);

                if (!product.IsAvailable)
                {
                    throw new ServiceLayerApiException($"Product with id {product.Id} is not available.", HttpStatusCode.Conflict);
                }

                await _productService.UpdateProductStockAsync(product, item.Quantity);

                var cartItem = new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price,
                };

                cartItem.Quantity = item.Quantity;
                cartItem.Price = product.Price;

                await _cartItemsRepository.AddAsync(cartItem);
            }
        }

        public async Task UpdateCartItems(Order order, IList<CartItemDTO> items)
        {
            foreach (var item in items)
            {
                var product = await _productService.GetProductAsync(item.ProductId);

                if (!product.IsAvailable)
                {
                    throw new ServiceLayerApiException($"Product with id {product.Id} is not available.", HttpStatusCode.Conflict);
                }

                var cartItem = order.Cart.FirstOrDefault(x => x.ProductId == item.ProductId);

                if (cartItem == null)
                {
                    throw new EntityNotFoundApiException($"Product in cart with specified id {product.Id} hasn't been found");
                }

                var oldQuantity = cartItem.Quantity;
                var newQuantity = item.Quantity;

                await _productService.UpdateProductStockAsync(product, newQuantity - oldQuantity);

                cartItem.Quantity = newQuantity;
                cartItem.Price = product.Price;

                await _cartItemsRepository.UpdateAsync(cartItem);
            }
        }

        public async Task DeleteCartItems(Order order, IList<CartItemDTO> items)
        {
            foreach (var item in items)
            {
                var product = await _productService.GetProductAsync(item.ProductId);

                await _productService.UpdateProductStockAsync(product, -item.Quantity);

                var cartItem = order.Cart.First(x => x.ProductId == item.ProductId);

                await _cartItemsRepository.DeleteAsync(cartItem);
            }
        }
    }
}
