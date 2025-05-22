using Advertising_Agency.Domain.Models;

namespace Advertising_Agency.BusinessLogic.Interfaces
{
    public interface IDiscountService
    {
        Task<DiscountDto> CreateDiscountAsync(DiscountDto discountDto);
        Task DeleteDiscountAsync(Guid discountId);
        Task<IEnumerable<DiscountDto>> GetAllDiscountsAsync();
        Task<DiscountDto> GetDiscountByIdAsync(Guid discountId);
        Task UpdateDiscountAsync(DiscountDto discountDto);
    }
}