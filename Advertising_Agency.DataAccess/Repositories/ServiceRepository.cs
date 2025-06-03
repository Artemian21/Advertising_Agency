using Advertising_Agency.DataAccess.Entities;
using Advertising_Agency.DataAccess.Interfaces;
using Advertising_Agency.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Advertising_Agency.DataAccess.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AdvertisingAgencyContext _context;

        public ServiceRepository(AdvertisingAgencyContext context)
        {
            _context = context;
        }

        public async Task<Service?> GetByIdAsync(Guid id) =>
            await _context.Services
                .Include(s => s.Discounts)
                .FirstOrDefaultAsync(s => s.ServiceId == id);

        public async Task<IEnumerable<Service>> GetAllAsync() =>
            await _context.Services
                .Include(s => s.Discounts)
                .ToListAsync();

        public async Task AddAsync(Service service)
        {
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Service service)
        {
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Service service)
        {
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
        }

        // Пошук послуг за ім'ям (частковий збіг)
        public async Task<IEnumerable<Service>> SearchByNameAsync(string searchTerm) =>
            await _context.Services
                .Where(s => s.Name.Contains(searchTerm))
                .Include(s => s.Discounts)
                .ToListAsync();

        // Фільтр за типом послуги
        public async Task<IEnumerable<Service>> GetByServiceTypeAsync(ServiceType serviceType) =>
            await _context.Services
                .Where(s => s.ServiceType == serviceType)
                .Include(s => s.Discounts)
                .ToListAsync();

        // Отримати послуги зі знижками (активні на певну дату)
        public async Task<IEnumerable<Service>> GetServicesWithActiveDiscountsAsync(DateTime date)
        {
            return await _context.Services
                .Where(s => s.Discounts.Any(d => d.ValidFrom <= date && d.ValidTo >= date))
                .Include(s => s.Discounts.Where(d => d.ValidFrom <= date && d.ValidTo >= date))
                .ToListAsync();
        }

        // Отримати "швидкі замовлення" (ServiceType == QuickOrder)
        public async Task<IEnumerable<Service>> GetQuickOrderServicesAsync() =>
            await GetByServiceTypeAsync(ServiceType.QuickOrder);
    }


}
