using Advertising_Agency.Domain.Models;

namespace Advertising_Agency.BusinessLogic.Interfaces
{
    public interface IOrderItemService
    {
        Task<OrderItemDto> CreateOrderItemAsync(OrderItemDto orderItemDto);
        Task DeleteOrderItemAsync(Guid orderItemId);
        Task<OrderItemDto> GetOrderItemByIdAsync(Guid orderItemId);
        Task<IEnumerable<OrderItemDto>> GetOrderItemsByOrderIdAsync(Guid orderId);
        Task UpdateOrderItemAsync(OrderItemDto orderItemDto);
    }
}