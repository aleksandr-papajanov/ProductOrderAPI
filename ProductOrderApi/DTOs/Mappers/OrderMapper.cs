using ProductOrderApi.DTOs.Orders;
using ProductOrderApi.Entities;

namespace ProductOrderApi.DTOs.Mappers
{
    internal static class OrderMapper
    {
        public static OrderDTO ToDTO(this Order entity)
        {
            return new OrderDTO
            {
                Id = entity.Id,
                Comment = entity.Comment,
                TotalPrice = entity.TotalPrice,
                Cart = entity.Cart.Select(e => e.ToDTO()).ToList(),
                Tracking = entity.Tracking.ToDictionary(e => e.UpdatedAt, e => OrderStatusLabels.Parse(e.Status)),
            };
        }

        public static OrderProductDTO ToDTO(this OrderProduct entity)
        {
            return new OrderProductDTO
            {
                Price = entity.Price,
                Quantity = entity.Quantity,
                Product = entity.Product.ToCartDTO()
            };
        }

        public static CartItemDTO ToCartItemDTO(this OrderProduct entity)
        {
            return new CartItemDTO
            {
                Quantity = entity.Quantity,
                ProductId = entity.Product.Id
            };
        }

        public static OrderProduct ToEntity(this CartItemDTO dto, int orderId)
        {
            return new OrderProduct
            {
                OrderId = orderId,
                Quantity = dto.Quantity,
                ProductId = dto.ProductId
            };
        }
    }
}
