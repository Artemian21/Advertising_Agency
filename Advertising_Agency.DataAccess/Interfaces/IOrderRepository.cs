using Advertising_Agency.DataAccess.Entities;

namespace Advertising_Agency.DataAccess.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task DeleteAsync(Order order);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
        Task UpdateAsync(Order order);
    }
}