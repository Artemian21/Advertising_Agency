using Advertising_Agency.BusinessLogic.Services;
using Advertising_Agency.DataAccess.Entities;
using Advertising_Agency.DataAccess.Interfaces;
using Advertising_Agency.Domain.Enums;
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
    public class ServiceServiceTests
    {
        private readonly IFixture _fixture;
        private readonly IServiceRepository _repository;
        private readonly IMapper _mapper;
        private readonly ServiceService _sut;

        public ServiceServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _repository = _fixture.Freeze<IServiceRepository>();
            _mapper = _fixture.Freeze<IMapper>();

            _sut = new ServiceService(_repository, _mapper);
        }

        [Fact]
        public async Task GetAllServicesAsync_ReturnsMappedServices()
        {
            var services = _fixture.CreateMany<Service>(3);
            var dtos = _fixture.CreateMany<ServiceDto>(3);

            _repository.GetAllAsync().Returns(services);
            _mapper.Map<IEnumerable<ServiceDto>>(services).Returns(dtos);

            var result = await _sut.GetAllServicesAsync();

            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task GetServiceByIdAsync_ReturnsMappedService_WhenFound()
        {
            var service = _fixture.Create<Service>();
            var dto = _fixture.Create<ServiceDto>();

            _repository.GetByIdAsync(service.ServiceId).Returns(service);
            _mapper.Map<ServiceDto>(service).Returns(dto);

            var result = await _sut.GetServiceByIdAsync(service.ServiceId);

            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task GetServiceByIdAsync_ThrowsException_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _repository.GetByIdAsync(id).Returns((Service)null);

            var act = async () => await _sut.GetServiceByIdAsync(id);

            var ex = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Service not found", ex.Message);
        }

        [Fact]
        public async Task CreateServiceAsync_ReturnsMappedDto()
        {
            var dto = _fixture.Create<ServiceDto>();
            var entity = _fixture.Create<Service>();

            _mapper.Map<Service>(dto).Returns(entity);
            _mapper.Map<ServiceDto>(entity).Returns(dto);

            var result = await _sut.CreateServiceAsync(dto);

            await _repository.Received(1).AddAsync(entity);
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task UpdateServiceAsync_Updates_WhenFound()
        {
            var dto = _fixture.Create<ServiceDto>();
            var entity = _fixture.Create<Service>();

            _repository.GetByIdAsync(dto.ServiceId).Returns(entity);

            await _sut.UpdateServiceAsync(dto);

            _mapper.Received(1).Map(dto, entity);
            await _repository.Received(1).UpdateAsync(entity);
        }

        [Fact]
        public async Task UpdateServiceAsync_ThrowsException_WhenNotFound()
        {
            var dto = _fixture.Create<ServiceDto>();
            _repository.GetByIdAsync(dto.ServiceId).Returns((Service)null);

            var act = async () => await _sut.UpdateServiceAsync(dto);

            var ex = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Service not found", ex.Message);
        }

        [Fact]
        public async Task DeleteServiceAsync_Deletes_WhenFound()
        {
            var entity = _fixture.Create<Service>();

            _repository.GetByIdAsync(entity.ServiceId).Returns(entity);

            await _sut.DeleteServiceAsync(entity.ServiceId);

            await _repository.Received(1).DeleteAsync(entity);
        }

        [Fact]
        public async Task DeleteServiceAsync_ThrowsException_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _repository.GetByIdAsync(id).Returns((Service)null);

            var act = async () => await _sut.DeleteServiceAsync(id);

            var ex = await Assert.ThrowsAsync<Exception>(act);
            Assert.Equal("Service not found", ex.Message);
        }

        [Fact]
        public async Task GetServicesByTypeAsync_ReturnsMapped()
        {
            var type = _fixture.Create<ServiceType>();
            var services = _fixture.CreateMany<Service>(3);
            var dtos = _fixture.CreateMany<ServiceDto>(3);

            _repository.GetByServiceTypeAsync(type).Returns(services);
            _mapper.Map<IEnumerable<ServiceDto>>(services).Returns(dtos);

            var result = await _sut.GetServicesByTypeAsync(type);

            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task SearchServicesAsync_ReturnsEmptyList_WhenSearchTermIsNullOrWhiteSpace()
        {
            var result1 = await _sut.SearchServicesAsync(null);
            var result2 = await _sut.SearchServicesAsync("  ");

            Assert.Empty(result1);
            Assert.Empty(result2);
        }

        [Fact]
        public async Task SearchServicesAsync_ReturnsMappedResults_WhenFound()
        {
            var searchTerm = "seo";
            var services = _fixture.CreateMany<Service>(2);
            var dtos = _fixture.CreateMany<ServiceDto>(2);

            _repository.SearchByNameAsync(searchTerm).Returns(services);
            _mapper.Map<IEnumerable<ServiceDto>>(services).Returns(dtos);

            var result = await _sut.SearchServicesAsync("  seo  ");

            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task GetServicesWithActiveDiscountAsync_ReturnsMapped()
        {
            var date = DateTime.UtcNow;
            var services = _fixture.CreateMany<Service>(2);
            var dtos = _fixture.CreateMany<ServiceDto>(2);

            _repository.GetServicesWithActiveDiscountsAsync(date).Returns(services);
            _mapper.Map<IEnumerable<ServiceDto>>(services).Returns(dtos);

            var result = await _sut.GetServicesWithActiveDiscountAsync(date);

            Assert.Equal(dtos, result);
        }
    }
}
