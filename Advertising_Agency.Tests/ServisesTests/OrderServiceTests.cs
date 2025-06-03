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
    public class OrderServiceTests
    {
        private readonly IFixture _fixture;
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly OrderService _sut;

        public OrderServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repository = _fixture.Freeze<IOrderRepository>();
            _mapper = _fixture.Freeze<IMapper>();

            _sut = new OrderService(_repository, _mapper);
        }

        [Fact]
        public async Task GetAllOrdersAsync_ReturnsMappedOrders()
        {
            var orders = _fixture.CreateMany<Order>(3);
            var dtos = _fixture.CreateMany<OrderDto>(3);

            _repository.GetAllAsync().Returns(orders);
            _mapper.Map<IEnumerable<OrderDto>>(orders).Returns(dtos);

            var result = await _sut.GetAllOrdersAsync();

            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ReturnsMappedOrder_WhenFound()
        {
            var order = _fixture.Create<Order>();
            var dto = _fixture.Create<OrderDto>();

            _repository.GetByIdAsync(order.OrderId).Returns(order);
            _mapper.Map<OrderDto>(order).Returns(dto);

            var result = await _sut.GetOrderByIdAsync(order.OrderId);

            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ThrowsException_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _repository.GetByIdAsync(id).Returns((Order)null);

            var act = async () => await _sut.GetOrderByIdAsync(id);

            var ex = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Order not found", ex.Message);
        }

        [Fact]
        public async Task CreateOrderAsync_ReturnsMappedDto()
        {
            var dto = _fixture.Create<OrderDto>();
            var entity = _fixture.Create<Order>();

            _mapper.Map<Order>(dto).Returns(entity);
            _mapper.Map<OrderDto>(entity).Returns(dto);

            var result = await _sut.CreateOrderAsync(dto);

            await _repository.Received(1).AddAsync(entity);
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task UpdateOrderAsync_Updates_WhenFound()
        {
            var dto = _fixture.Create<OrderDto>();
            var entity = _fixture.Create<Order>();

            _repository.GetByIdAsync(dto.OrderId).Returns(entity);

            await _sut.UpdateOrderAsync(dto);

            _mapper.Received(1).Map(dto, entity);
            await _repository.Received(1).UpdateAsync(entity);
        }

        [Fact]
        public async Task UpdateOrderAsync_ThrowsException_WhenNotFound()
        {
            var dto = _fixture.Create<OrderDto>();
            _repository.GetByIdAsync(dto.OrderId).Returns((Order)null);

            var act = async () => await _sut.UpdateOrderAsync(dto);

            var ex = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Order not found", ex.Message);
        }

        [Fact]
        public async Task DeleteOrderAsync_Deletes_WhenFound()
        {
            var entity = _fixture.Create<Order>();

            _repository.GetByIdAsync(entity.OrderId).Returns(entity);

            await _sut.DeleteOrderAsync(entity.OrderId);

            await _repository.Received(1).DeleteAsync(entity);
        }

        [Fact]
        public async Task DeleteOrderAsync_ThrowsException_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _repository.GetByIdAsync(id).Returns((Order)null);

            var act = async () => await _sut.DeleteOrderAsync(id);

            var ex = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Order not found", ex.Message);
        }
    }
}
