using Bootcamp.Clean.ApplicationService.ProductService.DTOs;
using Bootcamp.Clean.ApplicationService.ProductService.Helpers;
using Bootcamp.Clean.ApplicationService.SharedDto;
using Bootcamp.Clean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Clean.ApplicationService.Interfaces
{
    public interface IProductRepository
    {
        Task<ResponseModelDto<Guid>> Create(Product request);
        Task<ResponseModelDto<NoContent>> Update(Guid productId, ProductUpdateRequestDto request);

        Task<ResponseModelDto<NoContent>> UpdateProductName(Guid id, string name);

        Task<ResponseModelDto<NoContent>> Delete(Guid id);
        Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllWithCalculatedTax(PriceCalculator priceCalculator);
        Task<ResponseModelDto<ProductDto?>> GetByIdWithCalculatedTax(Guid id, PriceCalculator priceCalculator);
        Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllByPageWithCalculatedTax(
            PriceCalculator priceCalculator, int page, int pageSize);
        Task<ResponseModelDto<ProductDto?>> GetById(Guid id);
        Task<bool> HasExist(Guid id);
    }
}
