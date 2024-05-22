using AutoMapper;
using Bootcamp.Clean.ApplicationService.Interfaces;
using Bootcamp.Clean.ApplicationService.ProductService.DTOs;
using Bootcamp.Clean.ApplicationService.ProductService.Helpers;
using Bootcamp.Clean.ApplicationService.SharedDto;
using Bootcamp.Clean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Clean.ApplicationService.ProductService.Service
{
    public class ProductService(IProductRepository _productRepository, ICacheService cacheService)
    {
        public async Task<ResponseModelDto<Guid>> Create(ProductCreateRequestDto request)
        {
            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Price = request.Price,
                Stock = 10,
                Barcode = Guid.NewGuid().ToString(),
                Created = DateTime.Now
            };

            var result =await _productRepository.Create(newProduct);

            return result;
        }

        public async Task<ResponseModelDto<NoContent>> Delete(Guid id)
        {
            var result = await _productRepository.Delete(id);

            return result;
        }

        public async Task<ResponseModelDto<ProductDto?>> GetById(Guid id)
        {
            var result = await _productRepository.GetById(id);

            return result;
        }

        public async Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllByPageWithCalculatedTax(
            PriceCalculator priceCalculator, int page, int pageSize)
        {
            var result = await _productRepository.GetAllByPageWithCalculatedTax(priceCalculator, page, pageSize);

            return result;
        }

        public async Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllWithCalculatedTax(
            PriceCalculator priceCalculator)
        {
            var result = await _productRepository.GetAllWithCalculatedTax(priceCalculator);

            return result;
        }

        public async Task<ResponseModelDto<ProductDto?>> GetByIdWithCalculatedTax(Guid id,
            PriceCalculator priceCalculator)
        {
            var result = await _productRepository.GetByIdWithCalculatedTax(id, priceCalculator);

            return result;
        }

        public async Task<ResponseModelDto<NoContent>> Update(Guid productId, ProductUpdateRequestDto request)
        {
            var result = await _productRepository.Update(productId, request);

            return result;
        }

        public async Task<ResponseModelDto<NoContent>> UpdateProductName(Guid id, string name)
        {
            var result = await _productRepository.UpdateProductName(id, name);

            return result;
        }
    }
}
