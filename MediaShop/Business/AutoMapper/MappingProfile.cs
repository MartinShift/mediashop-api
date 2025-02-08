using AutoMapper;
using MediaShop.Business.Models;
using MediaShop.Data.Entities;
using MediaShop.Data.Enums;

namespace MediaShop.Business.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>();
        CreateMap<User, UserDto>()
              .ForMember(dest => dest.OrderCount, opt => opt.MapFrom(src => src.Orders.Where(x=> x.Status == OrderStatus.InProgress).Count()));
        CreateMap<User, ProfileDto>()
                          .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
        src.Reviews.Any() ? (double?)src.Products.Where(x => x.Reviews.Count > 0).Average(x => x.Reviews.Average(x => x.Rating)) : null));

        CreateMap<ProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.Reviews, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<Product, ProductDto>()
              .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
        src.Reviews.Any() ? (double?)src.Reviews.Average(x => x.Rating) : null))
              .ForMember(dest => dest.RatingCount, opt => opt.MapFrom(src => src.Reviews.Count()));

        CreateMap<ReviewDto, Review>();
        CreateMap<Review, ReviewDto>();

        CreateMap<CategoryDto, Category>();
        CreateMap<Category, CategoryDto>();

        CreateMap<RoleDto, Role>();
        CreateMap<Role, RoleDto>();

        CreateMap<OrderDto, Order>();
        CreateMap<Order, OrderDto>();

    }
}
