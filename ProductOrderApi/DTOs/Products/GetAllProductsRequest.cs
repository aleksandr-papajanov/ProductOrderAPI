using ProductOrderApi.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ProductOrderApi.DTOs.Products
{
    /// <summary>
    /// Command to retrieve a paginated list of products with optional sorting and filtering.
    /// </summary>
    public class GetAllProductsRequest
    {
        /// <summary>
        /// The number of products to skip in the query results. Default is 0.
        /// </summary>
        /// <remarks>
        /// Use this parameter for pagination. 
        /// For example, if you want the second page of results with 10 items per page, set Skip to 10.
        /// </remarks>
        [Range(0, int.MaxValue)]
        public int Skip { get; set; } = 0;

        /// <summary>
        /// The number of products to take in the query results. Default is 10. Maximum is 100.
        /// </summary>
        /// <remarks>
        /// Use this parameter to limit the number of results returned in the response.
        /// </remarks>
        [Range(1, 100)]
        public int Take { get; set; } = 10;

        /// <summary>
        /// The field by which the results should be sorted.
        /// </summary>
        /// <remarks>
        /// Allowed fields: Id, Name, Code, Price, CreatedAt, ModifiedAt.
        /// </remarks>
        [ValidFields("Id", "Name", "Code", "Price", "CreatedAt", "ModifiedAt")]
        public string? OrderBy { get; set; }

        /// <summary>
        /// The direction of sorting: ascending (ASC) or descending (DESC). Default is ASC.
        /// </summary>
        /// <remarks>
        /// Allowed values: ASC, DESC.
        /// </remarks>
        [ValidFields("ASC", "DESC")]
        public string OrderDirection { get; set; } = "ASC";

        /// <summary>
        /// A filter string to search within the specified fields.
        /// </summary>
        /// <remarks>
        /// Use this to narrow down the results based on a search term.
        /// </remarks>
        public string? Filter { get; set; }

        /// <summary>
        /// The field by which the filtering should be applied.
        /// </summary>
        /// <remarks>
        /// Allowed fields: Name, Code, Description, Features.
        /// </remarks>
        [ValidFields("Name", "Code", "Description", "Features")]
        public string? FilterBy { get; set; }
    }
}