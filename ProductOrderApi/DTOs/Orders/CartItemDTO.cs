using System.ComponentModel.DataAnnotations;

namespace ProductOrderApi.DTOs.Orders
{
    /// <summary>
    /// Data Transfer Object for a cart item in an order.
    /// </summary>
    public class CartItemDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// The product must be available in the system.
        /// </summary>
        /// <example>1</example>
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product in the cart.
        /// The value must be at least 1.
        /// </summary>
        /// <example>2</example>
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
