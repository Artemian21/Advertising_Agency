using Advertising_Agency.DataAccess.Entities;
using Advertising_Agency.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertising_Agency.DataAccess.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly AdvertisingAgencyContext _context;

        public DiscountRepository(AdvertisingAgencyContext context)
        {
            _context = context;
        }

        public async Task<Discount?> GetByIdAsync(Guid id) =>
            await _context.Discounts
                .Include(d => d.Service)
                .FirstOrDefaultAsync(d => d.DiscountId == id);

        public async Task<IEnumerable<Discount>> GetAllAsync() =>
            await _context.Discounts
                .Include(d => d.Service)
                .ToListAsync();

        public async Task AddAsync(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Discount discount)
        {
            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Discount discount)
        {
            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
        }

        // Отримати активні знижки на певну дату
        public async Task<IEnumerable<Discount>> GetActiveDiscountsAsync(DateTime date) =>
            await _context.Discounts
                .Where(d => d.ValidFrom <= date && d.ValidTo >= date)
                .Include(d => d.Service)
                .ToListAsync();

        // Отримати знижки за ServiceId
        public async Task<IEnumerable<Discount>> GetDiscountsByServiceIdAsync(Guid serviceId) =>
            await _context.Discounts
                .Where(d => d.ServiceId == serviceId)
                .ToListAsync();
    }
}
