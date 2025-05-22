using Advertising_Agency.DataAccess.Entities;

namespace Advertising_Agency.DataAccess.Interfaces
{
    public interface IOrderItemRepository
    {
        Task AddAsync(OrderItem orderItem);
        Task DeleteAsync(OrderItem orderItem);
        Task<OrderItem?> GetByIdAsync(Guid id);
        Task UpdateAsync(OrderItem orderItem);
        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(Guid orderId);
    }
}