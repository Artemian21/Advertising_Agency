using Advertising_Agency.BusinessLogic.Interfaces;
using Advertising_Agency.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Advertising_Agency.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Manager")] // Тільки менеджер і адмін можуть бачити всі замовлення
        public async Task<IActionResult> GetAll() =>
            Ok(await _orderService.GetAllOrdersAsync());

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,Manager,RegisteredUser")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "RegisteredUser")] // Тільки зареєстровані можуть створювати замовлення
        public async Task<IActionResult> Create([FromBody] OrderDto dto)
        {
            try
            {
                var created = await _orderService.CreateOrderAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.OrderId }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderDto dto)
        {
            if (id != dto.OrderId)
                return BadRequest(new { message = "ID mismatch" });

            try
            {
                await _orderService.UpdateOrderAsync(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
