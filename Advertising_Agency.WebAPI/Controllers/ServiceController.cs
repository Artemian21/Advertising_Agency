using Advertising_Agency.BusinessLogic.Interfaces;
using Advertising_Agency.Domain.Enums;
using Advertising_Agency.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Advertising_Agency.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetAllServices()
        {
            var services = await _serviceService.GetAllServicesAsync();
            return Ok(services);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ServiceDto>> GetServiceById(Guid id)
        {
            try
            {
                var service = await _serviceService.GetServiceByIdAsync(id);
                return Ok(service);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<ActionResult<ServiceDto>> CreateService(ServiceDto serviceDto)
        {
            var createdService = await _serviceService.CreateServiceAsync(serviceDto);
            return CreatedAtAction(nameof(GetServiceById), new { id = createdService.ServiceId }, createdService);
        }

        [HttpPut]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<IActionResult> UpdateService(ServiceDto serviceDto)
        {
            try
            {
                await _serviceService.UpdateServiceAsync(serviceDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager,Administrator")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            try
            {
                await _serviceService.DeleteServiceAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("type/{serviceType}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServicesByType(ServiceType serviceType)
        {
            var services = await _serviceService.GetServicesByTypeAsync(serviceType);
            return Ok(services);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> SearchServices([FromQuery] string term)
        {
            var services = await _serviceService.SearchServicesAsync(term);
            return Ok(services);
        }

        [HttpGet("discounts")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServicesWithActiveDiscounts([FromQuery] DateTime date)
        {
            var services = await _serviceService.GetServicesWithActiveDiscountAsync(date);
            return Ok(services);
        }
    }
}
