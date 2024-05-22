using AutoMapper;
using Bootcamp.Clean.ApplicationService.ProductService.DTOs;
using Bootcamp.Clean.ApplicationService.SharedDto;
using Bootcamp.Clean.Domain.Entities;

namespace Bootcamp.Clean.ApplicationService.ProductService.Configurations
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(x => x.Created, opt => opt.MapFrom(y => y.Created.ToShortDateString()))
                .ReverseMap();

            CreateMap<Product, ProductCreateRequestDto>()
                .ReverseMap();
        }
    }
}
