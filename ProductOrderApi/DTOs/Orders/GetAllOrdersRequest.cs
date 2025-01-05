using ProductOrderApi.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ProductOrderApi.DTOs.Orders
{
    /// <summary>
    /// Represents the request parameters for retrieving a list of orders with pagination and filtering options.
    /// </summary>
    public class GetAllOrdersRequest
    {
        /// <summary>
        /// The number of items to skip when retrieving the orders. 
        /// Used for pagination.
        /// </summary>
        /// <remarks>
        /// This parameter should be a non-negative integer. The default value is 0.
        /// </remarks>
        [Range(0, int.MaxValue)]
        public int Skip { get; set; } = 0;

        /// <summary>
        /// The number of items to return in the result set. 
        /// Specifies the limit on the number of orders.
        /// </summary>
        /// <remarks>
        /// This parameter should be an integer between 1 and 100. The default value is 10.
        /// </remarks>
        [Range(1, 100)]
        public int Take { get; set; } = 10;

        /// <summary>
        /// The field by which to order the results.
        /// </summary>
        /// <remarks>
        /// Acceptable values are "TotalPrice" or "Status". 
        /// This parameter is used to specify the ordering of the returned orders.
        /// </remarks>
        [ValidFields("TotalPrice", "Status")]
        public string? OrderBy { get; set; }

        /// <summary>
        /// The direction in which to order the results.
        /// </summary>
        /// <remarks>
        /// Acceptable values are "ASC" (ascending) or "DESC" (descending). 
        /// The default value is "ASC".
        /// </remarks>
        [ValidFields("ASC", "DESC")]
        public string OrderDirection { get; set; } = "ASC";

        /// <summary>
        /// A string that can be used to filter the results.
        /// </summary>
        /// <remarks>
        /// This parameter allows for additional filtering on the orders. 
        /// The exact format of the filter depends on the implementation.
        /// </remarks>
        public string? Filter { get; set; }

        /// <summary>
        /// The field by which to filter the results.
        /// </summary>
        /// <remarks>
        /// Acceptable values are "Comment", "Status", "ProductName", or "ProductCode".
        /// This parameter is used to specify which field to filter by.
        /// </remarks>
        [ValidFields("Comment", "Status", "ProductName", "ProductCode")]
        public string? FilterBy { get; set; }
    }
}
