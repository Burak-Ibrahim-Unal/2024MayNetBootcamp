using AutoMapper;
using Bootcamp.Service.ProductService.DTOs;
using Domain.Entities;

namespace Bootcamp.Service.ProductService.Configurations
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            //.ForMember(x => x.Created, opt => opt.MapFrom(y => y.Created.ToShortDateString()))
            //.ForMember(x => x.Price, opt => opt.MapFrom(y => 200));
        }
    }
}
