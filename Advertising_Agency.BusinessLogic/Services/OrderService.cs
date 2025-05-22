using Advertising_Agency.BusinessLogic.Interfaces;
using Advertising_Agency.DataAccess.Entities;
using Advertising_Agency.DataAccess.Interfaces;
using Advertising_Agency.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertising_Agency.BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Order not found");

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            await _orderRepository.AddAsync(order);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task UpdateOrderAsync(OrderDto orderDto)
        {
            var existing = await _orderRepository.GetByIdAsync(orderDto.OrderId);
            if (existing == null) throw new Exception("Order not found");

            _mapper.Map(orderDto, existing); // оновлення полів через AutoMapper
            await _orderRepository.UpdateAsync(existing);
        }

        public async Task DeleteOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new Exception("Order not found");

            await _orderRepository.DeleteAsync(order);
        }
    }
}
