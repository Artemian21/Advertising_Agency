using Advertising_Agency.DataAccess.Entities;
using Advertising_Agency.Domain.Enums;

namespace Advertising_Agency.DataAccess.Interfaces
{
    public interface IServiceRepository
    {
        Task AddAsync(Service service);
        Task DeleteAsync(Service service);
        Task<IEnumerable<Service>> GetAllAsync();
        Task<Service?> GetByIdAsync(Guid id);
        Task<IEnumerable<Service>> GetByServiceTypeAsync(ServiceType serviceType);
        Task<IEnumerable<Service>> GetQuickOrderServicesAsync();
        Task<IEnumerable<Service>> GetServicesWithActiveDiscountsAsync(DateTime date);
        Task<IEnumerable<Service>> SearchByNameAsync(string searchTerm);
        Task UpdateAsync(Service service);
    }
}