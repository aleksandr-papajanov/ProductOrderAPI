using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductOrderApi.Commands.Products;
using ProductOrderApi.DTOs.Mappers;
using ProductOrderApi.DTOs.Products;

namespace ProductOrderApi.Controllers
{
    /// <summary>
    /// Handles product-related operations like fetching, creating, updating, and deleting products.
    /// </summary>
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// API controller for managing products.
        /// </summary>
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a list of all products.
        /// </summary>
        /// <param name="request">The request parameters for fetching all products.</param>
        /// <returns>A list of product DTOs.</returns>
        [Authorize(Roles = "Admin, Customer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll([FromQuery] GetAllProductsRequest request)
        {
            var command = new GetAllProductCommand(request);

            var products = await _mediator.Send(command);

            var result = products.Select(e => e.ToDTO()).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Gets a single product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to fetch.</param>
        /// <returns>A product DTO for the specified product.</returns>
        [Authorize(Roles = "Admin, Customer")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Get([FromRoute] int id)
        {
            var command = new GetProductCommand(id);

            var product = await _mediator.Send(command);

            var result = product.ToDTO();

            return Ok(result);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="request">The product data to create.</param>
        /// <returns>A created product DTO.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var command = new CreateProductCommand(request);

            var product = await _mediator.Send(command);

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product.ToDTO());
        }

        /// <summary>
        /// Updates an existing product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="request">The updated product data.</param>
        /// <returns>No content response.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductRequest request)
        {
            var command = new UpdateProductCommand(id, request);

            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>No content response.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteProductCommand(id);

            await _mediator.Send(command);

            return NoContent();
        }
    }
}