using Advertising_Agency.BusinessLogic.Interfaces;
using Advertising_Agency.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Advertising_Agency.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        [AllowAnonymous] // Перегляд доступний всім
        public async Task<IActionResult> GetAll() =>
            Ok(await _discountService.GetAllDiscountsAsync());

        [HttpGet("{id}")]
        [AllowAnonymous] // Перегляд доступний всім
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var discount = await _discountService.GetDiscountByIdAsync(id);
                return Ok(discount);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Administrator")] // Створення – тільки менеджер або адміністратор
        public async Task<IActionResult> Create([FromBody] DiscountDto dto)
        {
            try
            {
                var created = await _discountService.CreateDiscountAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.DiscountId }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DiscountDto dto)
        {
            if (id != dto.DiscountId)
                return BadRequest(new { message = "ID mismatch" });

            try
            {
                await _discountService.UpdateDiscountAsync(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")] // Видалення – тільки адміністратор
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _discountService.DeleteDiscountAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
