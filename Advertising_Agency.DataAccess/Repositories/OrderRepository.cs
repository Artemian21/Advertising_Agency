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
    public class OrderRepository : IOrderRepository
    {
        private readonly AdvertisingAgencyContext _context;

        public OrderRepository(AdvertisingAgencyContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdAsync(Guid id) =>
            await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Service)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrderId == id);

        public async Task<IEnumerable<Order>> GetAllAsync() =>
            await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Service)
                .Include(o => o.User)
                .ToListAsync();

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        // Спеціальний метод - отримати всі замовлення користувача
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId) =>
            await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Service)
                .ToListAsync();

        // Фільтр замовлень за статусом
        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status) =>
            await _context.Orders
                .Where(o => o.Status == status)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Service)
                .ToListAsync();
    }
}
