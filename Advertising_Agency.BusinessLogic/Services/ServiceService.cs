using Advertising_Agency.BusinessLogic.Interfaces;
using Advertising_Agency.DataAccess.Entities;
using Advertising_Agency.DataAccess.Interfaces;
using Advertising_Agency.Domain.Enums;
using Advertising_Agency.Domain.Models;
using AutoMapper;

namespace Advertising_Agency.BusinessLogic.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public ServiceService(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceDto>> GetAllServicesAsync()
        {
            var services = await _serviceRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ServiceDto>>(services);
        }

        public async Task<ServiceDto> GetServiceByIdAsync(Guid id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null) throw new Exception("Service not found");

            return _mapper.Map<ServiceDto>(service);
        }

        public async Task<ServiceDto> CreateServiceAsync(ServiceDto serviceDto)
        {
            var service = _mapper.Map<Service>(serviceDto);
            await _serviceRepository.AddAsync(service);
            return _mapper.Map<ServiceDto>(service);
        }

        public async Task UpdateServiceAsync(ServiceDto serviceDto)
        {
            var existing = await _serviceRepository.GetByIdAsync(serviceDto.ServiceId);
            if (existing == null) throw new Exception("Service not found");

            _mapper.Map(serviceDto, existing);
            await _serviceRepository.UpdateAsync(existing);
        }

        public async Task DeleteServiceAsync(Guid serviceId)
        {
            var service = await _serviceRepository.GetByIdAsync(serviceId);
            if (service == null) throw new Exception("Service not found");

            await _serviceRepository.DeleteAsync(service);
        }

        public async Task<IEnumerable<ServiceDto>> GetServicesByTypeAsync(ServiceType serviceType)
        {
            var services = await _serviceRepository.GetByServiceTypeAsync(serviceType);
            return _mapper.Map<IEnumerable<ServiceDto>>(services);
        }

        public async Task<IEnumerable<ServiceDto>> SearchServicesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<ServiceDto>();

            var services = await _serviceRepository.SearchByNameAsync(searchTerm.Trim());
            return _mapper.Map<IEnumerable<ServiceDto>>(services);
        }

        public async Task<IEnumerable<ServiceDto>> GetServicesWithActiveDiscountAsync(DateTime date)
        {
            var services = await _serviceRepository.GetServicesWithActiveDiscountsAsync(date);
            return _mapper.Map<IEnumerable<ServiceDto>>(services);
        }
    }
}
