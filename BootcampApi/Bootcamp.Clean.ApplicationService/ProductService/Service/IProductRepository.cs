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

namespace Bootcamp.Clean.ApplicationService.ProductService.Service
{
    public interface IProductRepository
    {
        Task<Guid> Create(Product request);
        Task Update(Guid productId, ProductUpdateRequestDto request);

        Task UpdateProductName(Guid id, string name);

        Task Delete(Guid id);
        Task<List<Product>> GetAllWithCalculatedTax(PriceCalculator priceCalculator);
        Task<Product?> GetByIdWithCalculatedTax(Guid id, PriceCalculator priceCalculator);
        Task<List<Product>> GetAllByPageWithCalculatedTax(
            PriceCalculator priceCalculator, int page, int pageSize);
        Task<Product?> GetById(Guid id);
        Task<bool> HasExist(Guid id);
    }
}
