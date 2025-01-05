using System.ComponentModel.DataAnnotations;

namespace ProductOrderApi.DTOs.Products
{

    /// <summary>
    /// Data Transfer Object for a product.
    /// </summary>
    public class ProductDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique code of the product.
        /// </summary>
        /// <example>PRD12345</example>
        public required string Code { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <example>Wireless Mouse</example>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets an optional description of the product.
        /// </summary>
        /// <example>Ergonomic wireless mouse with adjustable DPI settings.</example>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product available in stock.
        /// </summary>
        /// <example>50</example>
        public int QuantityInStock { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        /// <example>19.99</example>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the product.
        /// </summary>
        /// <example>2023-01-01T12:00:00</example>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the last modification date of the product.
        /// </summary>
        /// <example>2023-01-02T15:30:00</example>
        public DateTime ModifiedAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product is available.
        /// </summary>
        /// <example>true</example>
        public bool IsAvailable { get; set; } = true;

        /// <summary>
        /// Gets or sets the additional features of the product as a dictionary of key-value pairs.
        /// </summary>
        /// <example>
        /// { "Color": "Black", "Connectivity": "Bluetooth" }
        /// </example>
        public Dictionary<string, string>? Features { get; set; }
    }
}
