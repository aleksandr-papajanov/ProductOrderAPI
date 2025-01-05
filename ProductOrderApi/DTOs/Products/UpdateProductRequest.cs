using System.ComponentModel.DataAnnotations;

namespace ProductOrderApi.DTOs.Products
{

    /// <summary>
    /// Data Transfer Object for updating an existing product.
    /// </summary>
    public class UpdateProductRequest
    {
        /// <summary>
        /// Gets or sets the unique code of the product.
        /// If provided, updates the existing product code.
        /// </summary>
        /// <example>PRD12345</example>
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the name of the product. 
        /// The name must not exceed 256 characters.
        /// </summary>
        /// <example>Wireless Mouse</example>
        [StringLength(256)]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets an optional description of the product. 
        /// The description must not exceed 1024 characters.
        /// </summary>
        /// <example>Ergonomic wireless mouse with adjustable DPI settings.</example>
        [StringLength(1024)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product available in stock.
        /// The value must be at least 1. If null, the quantity is not updated.
        /// </summary>
        /// <example>50</example>
        [Range(1, int.MaxValue)]
        public int? QuantityInStock { get; set; }

        /// <summary>
        /// Gets or sets the price of the product. 
        /// The value must be greater than or equal to 0.01. If null, the price is not updated.
        /// </summary>
        /// <example>19.99</example>
        [Range(0.01, double.MaxValue)]
        public decimal? Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product is available.
        /// If null, the availability is not updated.
        /// </summary>
        /// <example>true</example>
        public bool? IsAvailable { get; set; }

        /// <summary>
        /// Gets or sets the additional features of the product as a dictionary of key-value pairs.
        /// If null, the features are not updated.
        /// </summary>
        /// <example>
        /// { "Color": "Black", "Connectivity": "Bluetooth" }
        /// </example>
        public Dictionary<string, string>? Features { get; set; }
    }
}
