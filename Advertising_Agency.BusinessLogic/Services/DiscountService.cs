using Advertising_Agency.BusinessLogic.Interfaces;
using Advertising_Agency.DataAccess.Entities;
using Advertising_Agency.DataAccess.Interfaces;
using Advertising_Agency.Domain.Models;
using AutoMapper;

namespace Advertising_Agency.BusinessLogic.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository discountRepository, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DiscountDto>> GetAllDiscountsAsync()
        {
            var discounts = await _discountRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DiscountDto>>(discounts);
        }

        public async Task<DiscountDto> GetDiscountByIdAsync(Guid discountId)
        {
            var discount = await _discountRepository.GetByIdAsync(discountId);
            if (discount == null) throw new Exception("Discount not found");

            return _mapper.Map<DiscountDto>(discount);
        }

        public async Task<DiscountDto> CreateDiscountAsync(DiscountDto discountDto)
        {
            var discount = _mapper.Map<Discount>(discountDto);
            await _discountRepository.AddAsync(discount);
            return _mapper.Map<DiscountDto>(discount);
        }

        public async Task UpdateDiscountAsync(DiscountDto discountDto)
        {
            var existing = await _discountRepository.GetByIdAsync(discountDto.DiscountId);
            if (existing == null) throw new Exception("Discount not found");

            _mapper.Map(discountDto, existing); // AutoMapper оновить поля
            await _discountRepository.UpdateAsync(existing);
        }

        public async Task DeleteDiscountAsync(Guid discountId)
        {
            var discount = await _discountRepository.GetByIdAsync(discountId);
            if (discount == null) throw new Exception("Discount not found");

            await _discountRepository.DeleteAsync(discount);
        }
    }
}
