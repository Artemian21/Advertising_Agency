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
    public class OrderItemServiceTests
    {
        private readonly IFixture _fixture;
        private readonly IOrderItemRepository _repository;
        private readonly IMapper _mapper;
        private readonly OrderItemService _sut;

        public OrderItemServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repository = _fixture.Freeze<IOrderItemRepository>();
            _mapper = _fixture.Freeze<IMapper>();

            _sut = new OrderItemService(_repository, _mapper);
        }

        [Fact]
        public async Task GetOrderItemsByOrderIdAsync_ReturnsMappedItems()
        {
            var orderId = Guid.NewGuid();
            var entities = _fixture.CreateMany<OrderItem>(3);
            var dtos = _fixture.CreateMany<OrderItemDto>(3);

            _repository.GetOrderItemsByOrderIdAsync(orderId).Returns(entities);
            _mapper.Map<IEnumerable<OrderItemDto>>(entities).Returns(dtos);

            var result = await _sut.GetOrderItemsByOrderIdAsync(orderId);

            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task GetOrderItemByIdAsync_ReturnsMappedItem_WhenFound()
        {
            var item = _fixture.Create<OrderItem>();
            var dto = _fixture.Create<OrderItemDto>();

            _repository.GetByIdAsync(item.OrderItemId).Returns(item);
            _mapper.Map<OrderItemDto>(item).Returns(dto);

            var result = await _sut.GetOrderItemByIdAsync(item.OrderItemId);

            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task GetOrderItemByIdAsync_ThrowsException_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _repository.GetByIdAsync(id).Returns((OrderItem)null);

            var act = async () => await _sut.GetOrderItemByIdAsync(id);

            var ex = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Order item not found", ex.Message);
        }

        [Fact]
        public async Task CreateOrderItemAsync_ReturnsMappedDto()
        {
            var dto = _fixture.Create<OrderItemDto>();
            var entity = _fixture.Create<OrderItem>();

            _mapper.Map<OrderItem>(dto).Returns(entity);
            _mapper.Map<OrderItemDto>(entity).Returns(dto);

            var result = await _sut.CreateOrderItemAsync(dto);

            await _repository.Received(1).AddAsync(entity);
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task UpdateOrderItemAsync_Updates_WhenFound()
        {
            var dto = _fixture.Create<OrderItemDto>();
            var entity = _fixture.Create<OrderItem>();

            _repository.GetByIdAsync(dto.OrderItemId).Returns(entity);

            await _sut.UpdateOrderItemAsync(dto);

            _mapper.Received(1).Map(dto, entity);
            await _repository.Received(1).UpdateAsync(entity);
        }

        [Fact]
        public async Task UpdateOrderItemAsync_ThrowsException_WhenNotFound()
        {
            var dto = _fixture.Create<OrderItemDto>();
            _repository.GetByIdAsync(dto.OrderItemId).Returns((OrderItem)null);

            var act = async () => await _sut.UpdateOrderItemAsync(dto);

            var ex = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Order item not found", ex.Message);
        }

        [Fact]
        public async Task DeleteOrderItemAsync_Deletes_WhenFound()
        {
            var entity = _fixture.Create<OrderItem>();

            _repository.GetByIdAsync(entity.OrderItemId).Returns(entity);

            await _sut.DeleteOrderItemAsync(entity.OrderItemId);

            await _repository.Received(1).DeleteAsync(entity);
        }

        [Fact]
        public async Task DeleteOrderItemAsync_ThrowsException_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _repository.GetByIdAsync(id).Returns((OrderItem)null);

            var act = async () => await _sut.DeleteOrderItemAsync(id);

            var ex = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Order item not found", ex.Message);
        }
    }
}
