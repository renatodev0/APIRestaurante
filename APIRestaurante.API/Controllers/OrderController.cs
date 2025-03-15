using Microsoft.AspNetCore.Mvc;
using System.Net;
using APIRestaurante.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace APIRestaurante.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            if (createOrderDto == null)
            {
                return BadRequest("Invalid order data.");
            }

            try
            {
                var order = await _orderService.CreateOrderAsync(createOrderDto);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return Ok(order);
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderAsync(int id, [FromBody] UpdateOrderDto request)
        {
            try
            {
                var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized(new { message = "Usuário não autenticado." });
                }

                var order = await _orderService.UpdateOrderAsync(id, userId, request);
                if (order == null)
                {
                    return NotFound(new { message = "Pedido não encontrado." });
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{orderId}/items")]
        [Authorize]
        public async Task<IActionResult> AddOrderItemAsync(int orderId, [FromBody] AddOrderItemDto request)
        {
            try
            {
                var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized(new { message = "Usuário não autenticado." });
                }

                var order = await _orderService.AddOrderItemAsync(orderId, userId, request);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPatch("/items/{orderItemId}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderItemAsync(int orderItemId, [FromBody] UpdateOrderItemDto request)
        {
            try
            {
                var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized(new { message = "Usuário não autenticado." });
                }

                var order = await _orderService.UpdateOrderItemAsync(orderItemId, userId, request);
                if (order == null)
                {
                    return NotFound(new { message = "Item do pedido não encontrado." });
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("/items/{orderItemId}")]
        [Authorize]
        public async Task<IActionResult> RemoveOrderItemAsync(int orderItemId)
        {
            try
            {
                var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized(new { message = "Usuário não autenticado." });
                }

                var order = await _orderService.RemoveOrderItemAsync(orderItemId, userId);
                if (order == null)
                {
                    return NotFound(new { message = "Item do pedido não encontrado." });
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
