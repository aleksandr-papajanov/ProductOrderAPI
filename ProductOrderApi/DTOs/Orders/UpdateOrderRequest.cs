using Microsoft.AspNetCore.Http.HttpResults;
using ProductOrderApi.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ProductOrderApi.DTOs.Orders
{

    /// <summary>
    /// Data Transfer Object for updating an existing order.
    /// </summary>
    public class UpdateOrderRequest
    {
        /// <summary>
        /// Gets or sets an optional comment for the order update.
        /// The comment must not exceed 1024 characters.
        /// </summary>
        /// <example>Update the delivery address for the order.</example>
        [StringLength(1024)]
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets the list of cart items for the order update.
        /// If present, it represents the updated products and their quantities in the order.
        /// </summary>
        /// <example>
        /// [
        ///     { "ProductId": 1, "Quantity": 3 },
        ///     { "ProductId": 2, "Quantity": 2 }
        /// ]
        /// </example>
        public List<CartItemDTO>? Cart { get; set; }

        /// <summary>
        /// Gets or sets the status of the order.
        /// The status must be one of the following: "Created", "Confirmed", "Processing", "Shipped", "Delivered", "Cancelled", "Returned", "Refunded".
        /// </summary>
        /// <example>Shipped</example>
        [ValidFields("Created", "Confirmed", "Processing", "Shipped", "Delivered", "Cancelled", "Returned", "Refunded")]
        public string? Status { get; set; }
    }
}
