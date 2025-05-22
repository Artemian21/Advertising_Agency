using Advertising_Agency.BusinessLogic.Interfaces;
using Advertising_Agency.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Advertising_Agency.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet("by-order/{orderId}")]
        public async Task<IActionResult> GetByOrderId(Guid orderId)
        {
            var items = await _orderItemService.GetOrderItemsByOrderIdAsync(orderId);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var item = await _orderItemService.GetOrderItemByIdAsync(id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderItemDto dto)
        {
            try
            {
                var created = await _orderItemService.CreateOrderItemAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.OrderItemId }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderItemDto dto)
        {
            if (id != dto.OrderItemId)
                return BadRequest(new { message = "ID mismatch" });

            try
            {
                await _orderItemService.UpdateOrderItemAsync(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _orderItemService.DeleteOrderItemAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
