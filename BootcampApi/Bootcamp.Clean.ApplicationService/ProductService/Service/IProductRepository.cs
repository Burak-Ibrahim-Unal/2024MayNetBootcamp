using Bootcamp.Clean.ApplicationService.ProductService.DTOs;
using Bootcamp.Clean.ApplicationService.ProductService.Helpers;
using Bootcamp.Clean.ApplicationService.SharedDto;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Clean.ApplicationService.ProductService.Service
{
    public interface IProductRepository
    {
        Task<ResponseModelDto<int>> Create(ProductCreateRequestDto request);
        Task<ResponseModelDto<NoContent>> Update(int productId, ProductUpdateRequestDto request);

        Task<ResponseModelDto<NoContent>> UpdateProductName(int id, string name);

        Task<ResponseModelDto<NoContent>> Delete(int id);
        Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllWithCalculatedTax(PriceCalculator priceCalculator);
        Task<ResponseModelDto<ProductDto?>> GetByIdWithCalculatedTax(int id, PriceCalculator priceCalculator);
        Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllByPageWithCalculatedTax(
            PriceCalculator priceCalculator, int page, int pageSize);

    }
}
