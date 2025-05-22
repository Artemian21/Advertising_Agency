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
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;

        public OrderItemService(IOrderItemRepository orderItemRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderItemDto>> GetOrderItemsByOrderIdAsync(Guid orderId)
        {
            var items = await _orderItemRepository.GetOrderItemsByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<OrderItemDto>>(items);
        }

        public async Task<OrderItemDto> GetOrderItemByIdAsync(Guid orderItemId)
        {
            var item = await _orderItemRepository.GetByIdAsync(orderItemId);
            if (item == null) throw new Exception("Order item not found");

            return _mapper.Map<OrderItemDto>(item);
        }

        public async Task<OrderItemDto> CreateOrderItemAsync(OrderItemDto orderItemDto)
        {
            var item = _mapper.Map<OrderItem>(orderItemDto);
            await _orderItemRepository.AddAsync(item);
            return _mapper.Map<OrderItemDto>(item);
        }

        public async Task UpdateOrderItemAsync(OrderItemDto orderItemDto)
        {
            var existing = await _orderItemRepository.GetByIdAsync(orderItemDto.OrderItemId);
            if (existing == null) throw new Exception("Order item not found");

            _mapper.Map(orderItemDto, existing); // AutoMapper оновлює поля
            await _orderItemRepository.UpdateAsync(existing);
        }

        public async Task DeleteOrderItemAsync(Guid orderItemId)
        {
            var item = await _orderItemRepository.GetByIdAsync(orderItemId);
            if (item == null) throw new Exception("Order item not found");

            await _orderItemRepository.DeleteAsync(item);
        }
    }
}
