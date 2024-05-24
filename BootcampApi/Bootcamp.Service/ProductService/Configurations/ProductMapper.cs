using AutoMapper;
using Bootcamp.Service.ProductService.DTOs;
using Bootcamp.Service.ProductService.Helpers;
using Domain.Entities;

namespace Bootcamp.Service.ProductService.Configurations
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDto>()
            //.ForMember(x => x.Created, opt => opt.MapFrom(y => y.Created.ToShortDateString()))
            //.ForMember(x => x.Price, opt => opt.MapFrom(y => 200))   
            .ForPath(x => x.Created, opt => opt.MapFrom(y => y.Created.ToShortDateString()))
            .ForPath(x => x.Price, opt => opt.MapFrom(y => new PriceCalculator().CalculateKdv(y.Price, 1.20m)))
            .ReverseMap();
        }
    }
}
