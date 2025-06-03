using Advertising_Agency.BusinessLogic.Services;
using Advertising_Agency.DataAccess.Entities;
using Advertising_Agency.DataAccess.Interfaces;
using Advertising_Agency.Domain.Models;
using AutoFixture.AutoNSubstitute;
using AutoFixture;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;

namespace Advertising_Agency.Tests.ServisesTests
{
    public class DiscountServiceTests
    {
        private readonly IFixture _fixture;
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        private readonly DiscountService _sut;

        public DiscountServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _discountRepository = _fixture.Freeze<IDiscountRepository>();
            _mapper = _fixture.Freeze<IMapper>();

            _sut = new DiscountService(_discountRepository, _mapper);
        }

        [Fact]
        public async Task GetAllDiscountsAsync_ShouldReturnMappedDiscounts()
        {
            var discounts = _fixture.CreateMany<Discount>(3);
            var discountDtos = _fixture.CreateMany<DiscountDto>(3);

            _discountRepository.GetAllAsync().Returns(Task.FromResult((IEnumerable<Discount>)discounts));
            _mapper.Map<IEnumerable<DiscountDto>>(discounts).Returns(discountDtos);

            var result = await _sut.GetAllDiscountsAsync();

            Assert.Equal(discountDtos, result);
        }

        [Fact]
        public async Task GetDiscountByIdAsync_ShouldReturnMappedDiscount_WhenExists()
        {
            var discount = _fixture.Create<Discount>();
            var dto = _fixture.Create<DiscountDto>();

            _discountRepository.GetByIdAsync(discount.DiscountId).Returns(discount);
            _mapper.Map<DiscountDto>(discount).Returns(dto);

            var result = await _sut.GetDiscountByIdAsync(discount.DiscountId);

            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task GetDiscountByIdAsync_ShouldThrow_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _discountRepository.GetByIdAsync(id).Returns((Discount)null);

            var act = async () => await _sut.GetDiscountByIdAsync(id);

            var ex = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Discount not found", ex.Message);
        }

        [Fact]
        public async Task CreateDiscountAsync_ShouldReturnMappedDiscount()
        {
            var dto = _fixture.Create<DiscountDto>();
            var entity = _fixture.Create<Discount>();

            _mapper.Map<Discount>(dto).Returns(entity);
            _mapper.Map<DiscountDto>(entity).Returns(dto);

            var result = await _sut.CreateDiscountAsync(dto);

            await _discountRepository.Received(1).AddAsync(entity);
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task UpdateDiscountAsync_ShouldUpdate_WhenExists()
        {
            var dto = _fixture.Create<DiscountDto>();
            var entity = _fixture.Create<Discount>();

            _discountRepository.GetByIdAsync(dto.DiscountId).Returns(entity);

            await _sut.UpdateDiscountAsync(dto);

            _mapper.Received(1).Map(dto, entity);
            await _discountRepository.Received(1).UpdateAsync(entity);
        }

        [Fact]
        public async Task UpdateDiscountAsync_ShouldThrow_WhenNotFound()
        {
            var dto = _fixture.Create<DiscountDto>();
            _discountRepository.GetByIdAsync(dto.DiscountId).Returns((Discount)null);

            var act = async () => await _sut.UpdateDiscountAsync(dto);

            var ex = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Discount not found", ex.Message);
        }

        [Fact]
        public async Task DeleteDiscountAsync_ShouldDelete_WhenExists()
        {
            var discount = _fixture.Create<Discount>();
            _discountRepository.GetByIdAsync(discount.DiscountId).Returns(discount);

            await _sut.DeleteDiscountAsync(discount.DiscountId);

            await _discountRepository.Received(1).DeleteAsync(discount);
        }

        [Fact]
        public async Task DeleteDiscountAsync_ShouldThrow_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _discountRepository.GetByIdAsync(id).Returns((Discount)null);

            var act = async () => await _sut.DeleteDiscountAsync(id);

            var ex = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Discount not found", ex.Message);
        }
    }
}
