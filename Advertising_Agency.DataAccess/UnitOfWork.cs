using Advertising_Agency.DataAccess.Interfaces;
using Advertising_Agency.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertising_Agency.DataAccess
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly AdvertisingAgencyContext _context;

        public IUserRepository Users { get; }
        public IServiceRepository Services { get; }
        public IOrderRepository Orders { get; }
        public IOrderItemRepository OrderItems { get; }
        public IDiscountRepository Discounts { get; }

        public UnitOfWork(AdvertisingAgencyContext context)
        {
            _context = context;

            Users = new UserRepository(_context);
            Services = new ServiceRepository(_context);
            Orders = new OrderRepository(_context);
            OrderItems = new OrderItemRepository(_context);
            Discounts = new DiscountRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
