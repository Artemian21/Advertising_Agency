using Advertising_Agency.Domain.Enums;
using Advertising_Agency.Domain.Models;

namespace Advertising_Agency.BusinessLogic.Interfaces
{
    public interface IServiceService
    {
        Task<ServiceDto> CreateServiceAsync(ServiceDto serviceDto);
        Task DeleteServiceAsync(Guid serviceId);
        Task<IEnumerable<ServiceDto>> GetAllServicesAsync();
        Task<ServiceDto> GetServiceByIdAsync(Guid id);
        Task<IEnumerable<ServiceDto>> GetServicesByTypeAsync(ServiceType serviceType);
        Task<IEnumerable<ServiceDto>> GetServicesWithActiveDiscountAsync(DateTime date);
        Task<IEnumerable<ServiceDto>> SearchServicesAsync(string searchTerm);
        Task UpdateServiceAsync(ServiceDto serviceDto);
    }
}