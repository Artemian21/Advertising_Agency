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
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AdvertisingAgencyContext _context;

        public OrderItemRepository(AdvertisingAgencyContext context)
        {
            _context = context;
        }

        public async Task<OrderItem?> GetByIdAsync(Guid id) =>
            await _context.OrderItems
                .Include(oi => oi.Service)
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == id);

        public async Task AddAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(OrderItem orderItem)
        {
            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(Guid orderId)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }
    }
}
