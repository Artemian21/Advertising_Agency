using Advertising_Agency.DataAccess.Entities;

namespace Advertising_Agency.DataAccess.Interfaces
{
    public interface IDiscountRepository
    {
        Task AddAsync(Discount discount);
        Task DeleteAsync(Discount discount);
        Task<IEnumerable<Discount>> GetActiveDiscountsAsync(DateTime date);
        Task<IEnumerable<Discount>> GetAllAsync();
        Task<Discount?> GetByIdAsync(Guid id);
        Task<IEnumerable<Discount>> GetDiscountsByServiceIdAsync(Guid serviceId);
        Task UpdateAsync(Discount discount);
    }
}