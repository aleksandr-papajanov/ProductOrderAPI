namespace ProductOrderApi.DTOs.Products
{
    /// <summary>
    /// Represents a product in the shopping cart, including its unique identifier, code, and name.
    /// </summary>
    public class ProductInCartDTO
    {
        /// <summary>
        /// The unique identifier of the product in the cart.
        /// </summary>
        /// <remarks>
        /// This field is used to uniquely identify the product in the cart.
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        /// The unique code of the product in the cart.
        /// </summary>
        /// <remarks>
        /// This field is used for tracking or identifying the product within the cart system.
        /// </remarks>
        public required string Code { get; set; }

        /// <summary>
        /// The name of the product in the cart.
        /// </summary>
        /// <remarks>
        /// This field represents the display name of the product as it appears in the cart.
        /// </remarks>
        public required string Name { get; set; }
    }
}
