

namespace ProductOrderApi.DTOs.Orders
{
    /// <summary>
    /// Data Transfer Object for an order.
    /// </summary>
    public class OrderDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the order.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets an optional comment associated with the order.
        /// </summary>
        /// <example>Gift wrapping requested</example>
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets the total price of the order.
        /// </summary>
        /// <example>199.99</example>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets the list of products in the order's cart.
        /// </summary>
        /// <example>
        /// [
        ///     { "ProductId": 1, "Quantity": 2 },
        ///     { "ProductId": 3, "Quantity": 1 }
        /// ]
        /// </example>
        public List<OrderProductDTO> Cart { get; set; } = new();

        /// <summary>
        /// Gets or sets the tracking history of the order as a dictionary of timestamps and status.
        /// </summary>
        /// <example>
        /// { "2023-01-01T12:00:00": "Created", "2023-01-02T14:00:00": "Shipped" }
        /// </example>
        public Dictionary<DateTime, string> Tracking { get; set; } = new();
    }
}
