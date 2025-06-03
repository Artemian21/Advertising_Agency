using Advertising_Agency.DataAccess.Entities;
using Advertising_Agency.Domain.Models;
using AutoMapper;

namespace Advertising_Agency.DataAccess
{
    public class AdvertisingAgencyProfile : Profile
    {
        public AdvertisingAgencyProfile()
        {
            // User
            CreateMap<User, UserDto>()
                .ReverseMap()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<User, UserRegistrationDto>().ReverseMap();
            CreateMap<User, LoginDto>().ReverseMap();

            // Service
            CreateMap<Service, ServiceDto>().ReverseMap();

            // OrderItem
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service.Name))
                .ReverseMap()
                .ForMember(dest => dest.Service, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore());

            // Order
            CreateMap<Order, OrderDto>().ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // Discount
            CreateMap<Discount, DiscountDto>().ReverseMap()
                .ForMember(dest => dest.Service, opt => opt.Ignore());
        }
    }
}
