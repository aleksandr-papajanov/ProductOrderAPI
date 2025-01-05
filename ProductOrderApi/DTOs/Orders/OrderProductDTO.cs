using ProductOrderApi.DTOs.Products;
using System.ComponentModel.DataAnnotations;

namespace ProductOrderApi.DTOs.Orders
{
    /// <summary>
    /// Data Transfer Object for a product in an order's cart.
    /// </summary>
    public class OrderProductDTO
    {
        /// <summary>
        /// Gets or sets the quantity of the product in the order.
        /// The value must be at least 1.
        /// </summary>
        /// <example>2</example>
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// The value must be greater than or equal to 0.01.
        /// </summary>
        /// <example>19.99</example>
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the product details for the product in the cart.
        /// </summary>
        public ProductInCartDTO Product { get; set; } = null!;
    }
}
