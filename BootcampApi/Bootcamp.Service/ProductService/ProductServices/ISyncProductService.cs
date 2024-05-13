using Bootcamp.Service.ProductService.DTOs;
using Bootcamp.Service.ProductService.Helpers;
using Bootcamp.Service.SharedDto;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Service.ProductService.ProductServices
{
    public interface ISyncProductService
    {
        ResponseModelDto<ImmutableList<ProductDto>> GetAllWithCalculatedTax(PriceCalculator priceCalculator);

        ResponseModelDto<ProductDto?> GetByIdWithCalculatedTax(int id, PriceCalculator priceCalculator);

        ResponseModelDto<ImmutableList<ProductDto>> GetAllByPageWithCalculatedTax(
            PriceCalculator priceCalculator, int page, int pageSize);

        ResponseModelDto<int> Create(ProductCreateRequestDto request);
        ResponseModelDto<NoContent> Update(int productId, ProductUpdateRequestDto request);

        ResponseModelDto<NoContent> UpdateProductName(int id, string name);

        ResponseModelDto<NoContent> Delete(int id);
    }
}
