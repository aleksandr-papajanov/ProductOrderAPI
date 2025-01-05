using System.ComponentModel.DataAnnotations;

namespace ProductOrderApi.DTOs.Orders
{

    /// <summary>
    /// Data Transfer Object for creating a new order.
    /// </summary>
    public class CreateOrderRequest
    {
        /// <summary>
        /// Gets or sets an optional comment for the order.
        /// The comment must not exceed 1024 characters.
        /// </summary>
        /// <example>Include a gift wrapping for the order.</example>
        [StringLength(1024)]
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets the list of cart items for the order.
        /// Each item in the cart represents a product and its quantity.
        /// </summary>
        /// <example>
        /// [
        ///     { "ProductId": 1, "Quantity": 2 },
        ///     { "ProductId": 3, "Quantity": 1 }
        /// ]
        /// </example>
        public List<CartItemDTO> Cart { get; set; } = null!;
    }
}
