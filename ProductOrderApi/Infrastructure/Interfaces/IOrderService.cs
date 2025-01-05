using ProductOrderApi.DTOs.Orders;
using ProductOrderApi.Entities;

namespace ProductOrderApi.Infrastructure.Interfaces
{
    internal interface IOrderService
    {
        IQueryable<Order> Orders { get; }

        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(Order order);
        Task<Order> GetOrderAsync(int id);
        Task UpdateOrderTotalPriceAsync(Order order);
        OrderStatus GetCurrentOrderStatus(Order order);
        Task UpdateOrderStatusAsync(Order order, OrderStatus newStatus);
        Task AddCartItems(Order order, IList<CartItemDTO> item);
        Task UpdateCartItems(Order order, IList<CartItemDTO> item);
        Task DeleteCartItems(Order order, IList<CartItemDTO> item);
    }
}
