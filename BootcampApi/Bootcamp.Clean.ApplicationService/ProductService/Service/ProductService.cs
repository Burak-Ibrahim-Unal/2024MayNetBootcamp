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
using System.Text.Json;
using System.Threading.Tasks;

namespace Bootcamp.Clean.ApplicationService.ProductService.Service
{
    public class ProductService(IProductRepository _productRepository, IUnitOfWork unitOfWork, ICacheService _cacheService, ICustomCacheService _anotherCacheService, IMapper _mapper)
    {
        private const string _productsRedisKey = "products";

        public async Task<ResponseModelDto<List<ProductDto>>> GetAllWithCalculatedTax(PriceCalculator priceCalculator)
        {
            if (await _anotherCacheService.KeyExistsAsync(_productsRedisKey))
            {
                var productListFromRedisAsJson = await _anotherCacheService.GetValueAsync(_productsRedisKey);
                var productListFromRedis = JsonSerializer.Deserialize<List<ProductDto>>(productListFromRedisAsJson!);
                return ResponseModelDto<List<ProductDto>>.Success(productListFromRedis!);
            }

            var productList = await _productRepository.GetAllWithCalculatedTax(priceCalculator);

            var productListAsJson = JsonSerializer.Serialize(productList);
            await _anotherCacheService.SetValueAsync(_productsRedisKey, productListAsJson);

            var mappedProducts = _mapper.Map<List<ProductDto>>(productList);
            return ResponseModelDto<List<ProductDto>>.Success(mappedProducts);
        }

        public async Task<ResponseModelDto<Guid>> Create(ProductCreateRequestDto request)
        {
            unitOfWork.BeginTransaction();

            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Price = request.Price,
                Stock = 10,
                Barcode = Guid.NewGuid().ToString(),
                Created = DateTime.Now
            };

            var result = await _productRepository.Create(newProduct);
            await unitOfWork.CommitAsync();

            return ResponseModelDto<Guid>.Success(result, HttpStatusCode.Created);
        }

        public async Task<ResponseModelDto<NoContent>> Delete(Guid id)
        {
            unitOfWork.BeginTransaction();

            await _productRepository.Delete(id);

            await unitOfWork.CommitAsync();

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<ProductDto?>> GetById(Guid id)
        {
            //cache aside design pattern
            var productFromCache = _cacheService.Get<ProductDto?>($"product:{id}");

            if (productFromCache is not null)
                return ResponseModelDto<ProductDto?>.Success(productFromCache);

            var product = await _productRepository.GetById(id);
            _cacheService.Add($"product:{id}", new ProductDto(product.Id, product.Name, product.Price, product.Stock, product.Barcode, product.Created.ToShortTimeString()));

            var mappedProduct = _mapper.Map<ProductDto>(product);
            return ResponseModelDto<ProductDto?>.Success(mappedProduct);
        }

        public async Task<ResponseModelDto<List<ProductDto>>> GetAllByPageWithCalculatedTax(
            PriceCalculator priceCalculator, int page, int pageSize)
        {
            var productsList = await _productRepository.GetAllByPageWithCalculatedTax(priceCalculator, page, pageSize);
            var mappedProducts = _mapper.Map<List<ProductDto>>(productsList);

            return ResponseModelDto<List<ProductDto>>.Success(mappedProducts);
        }

        public async Task<ResponseModelDto<ProductDto?>> GetByIdWithCalculatedTax(Guid id,
            PriceCalculator priceCalculator)
        {
            var product = await _productRepository.GetByIdWithCalculatedTax(id, priceCalculator);
            var mappedProduct = _mapper.Map<ProductDto>(product);

            return ResponseModelDto<ProductDto?>.Success(mappedProduct);
        }

        public async Task<ResponseModelDto<NoContent>> Update(Guid productId, ProductUpdateRequestDto request)
        {
            unitOfWork.BeginTransaction();

            await _productRepository.Update(productId, request);

            await unitOfWork.CommitAsync();

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<NoContent>> UpdateProductName(Guid id, string name)
        {
            unitOfWork.BeginTransaction();

            await _productRepository.UpdateProductName(id, name);

            await unitOfWork.CommitAsync();

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}
