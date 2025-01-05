using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductOrderApi.Commands.Orders;
using ProductOrderApi.DTOs.Mappers;
using ProductOrderApi.DTOs.Orders;

namespace ProductOrderApi.Controllers
{
    /// <summary>
    /// API controller for managing orders.
    /// </summary>
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// API controller for managing orders.
        /// </summary>
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all orders.
        /// </summary>
        /// <param name="request">Query parameters for fetching orders.</param>
        /// <returns>List of orders.</returns>
        [Authorize(Roles = "Admin, Customer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAll([FromQuery] GetAllOrdersRequest request)
        {
            var command = new GetAllOrdersCommand(request);

            var orders = await _mediator.Send(command);

            var result = orders.Select(e => e.ToDTO());

            return Ok(result);
        }

        /// <summary>
        /// Get a specific order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>The requested order details.</returns>
        [Authorize(Roles = "Admin, Customer")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> Get([FromRoute] int id)
        {
            var command = new GetOrderCommand(id);

            var order = await _mediator.Send(command);

            var result = order.ToDTO();

            return Ok(result);
        }

        /// <summary>
        /// Create a new order.
        /// </summary>
        /// <param name="request">The order details to create.</param>
        /// <returns>The created order.</returns>
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var command = new CreateOrderCommand(request);

            var order = await _mediator.Send(command);

            return CreatedAtAction(nameof(Get), new { id = order.Id }, order.ToDTO());
        }

        /// <summary>
        /// Update an existing order.
        /// </summary>
        /// <param name="id">The ID of the order to update.</param>
        /// <param name="request">The updated order details.</param>
        /// <returns>No content response.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateOrderRequest request)
        {
            var command = new UpdateOrderCommand(id, request);

            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Delete an existing order by ID.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>No content response.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteOrderCommand(id);

            await _mediator.Send(command);

            return NoContent();
        }
    }
}