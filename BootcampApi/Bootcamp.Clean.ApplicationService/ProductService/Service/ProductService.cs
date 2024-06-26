﻿using AutoMapper;
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
    public class ProductService(IProductRepository _productRepository, IUnitOfWork _unitOfWork, ICacheService _cacheService, ICustomCacheService _customCacheService, IMapper _mapper)
    {
        private const string _productsRedisKey = "products";
        private const string _productsRedisKeyAsList = "products-list";
        private static PriceCalculator _priceCalculator;

        public async Task<ResponseModelDto<List<ProductDto>>> GetAllWithCalculatedTax(PriceCalculator priceCalculator)
        {
            if (await _customCacheService.KeyExistsAsync(_productsRedisKey))
            {
                #region 1.WAY
                /* 1.WAY Redis String => */
                var productListFromRedisAsJson = await _customCacheService.GetValueAsync(_productsRedisKey);

                #endregion
                var productListFromRedis = JsonSerializer.Deserialize<List<ProductDto>>(productListFromRedisAsJson!);
                return ResponseModelDto<List<ProductDto>>.Success(productListFromRedis!);
            }

            var productList = await _productRepository.GetAllWithCalculatedTax(priceCalculator);
            #region 1.WAY

            // 1.WAY Redis String => var productListAsJson = JsonSerializer.Serialize(productList);
            // 1.WAY Redis String =>  await _customCacheService.SetValueAsync(_productsRedisKey, productListAsJson); 
            #endregion

            /* 2.WAY Redis List => */
            productList.ForEach(product =>
            {
                _customCacheService.ListLeftPushAsync($"{_productsRedisKeyAsList}:{product.Id}", JsonSerializer.Serialize(product));
            });

            var mappedProducts = _mapper.Map<List<ProductDto>>(productList);
            return ResponseModelDto<List<ProductDto>>.Success(mappedProducts);
        }

        public async Task<ResponseModelDto<Guid>> Create(ProductCreateRequestDto request)
        {
            await _customCacheService.KeyDeleteAsync(_productsRedisKey);

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

            await _unitOfWork.CommitAsync();
            await AddProductsToRedis();

            return ResponseModelDto<Guid>.Success(result, HttpStatusCode.Created);
        }

        public async Task<ResponseModelDto<NoContent>> Delete(Guid id)
        {
            await _customCacheService.KeyDeleteAsync(_productsRedisKey);

            await _productRepository.Delete(id);

            await _unitOfWork.CommitAsync();
            await AddProductsToRedis();

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<ProductDto?>> GetById(Guid id)
        {
            //cache aside design pattern

            var redisKey = $"{_productsRedisKeyAsList}:{id}";
            #region InMemoryCache
            //var productFromCache = _cacheService.Get<ProductDto?>($"product:{id}");

            //if (productFromCache is not null)
            //    return ResponseModelDto<ProductDto?>.Success(productFromCache);
            #endregion

            #region RedisCache
            if (await _customCacheService.KeyExistsAsync($"{_productsRedisKeyAsList}:{id}"))
            {
                var productAsJsonFromRedis = await _customCacheService.ListGetByIndexAsync(redisKey);
                var productFromRedis = JsonSerializer.Deserialize<ProductDto>(productAsJsonFromRedis!);
                return ResponseModelDto<ProductDto?>.Success(productFromRedis);
            }
            #endregion

            var product = await _productRepository.GetById(id);

            #region InMemoryCache Add
            //_cacheService.Add($"product:{id}", new ProductDto(product.Id, product.Name, product.Price, product.Stock, product.Barcode, product.Created.ToShortTimeString())); 
            #endregion

            #region InRedisCache Add
            await _customCacheService.ListLeftPushAsync(redisKey, JsonSerializer.Serialize(product)); 
            #endregion

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
            await _customCacheService.KeyDeleteAsync(_productsRedisKey);

            await _productRepository.Update(productId, request);

            await _unitOfWork.CommitAsync();
            await AddProductsToRedis();

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<NoContent>> UpdateProductName(Guid id, string name)
        {
            await _customCacheService.KeyDeleteAsync(_productsRedisKey);

            await _productRepository.UpdateProductName(id, name);

            await _unitOfWork.CommitAsync();
            await AddProductsToRedis();

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        private async Task AddProductsToRedis()
        {
            var productList = await _productRepository.GetAllWithCalculatedTax(_priceCalculator);

            var productListAsJson = JsonSerializer.Serialize(productList);
            await _customCacheService.SetValueAsync(_productsRedisKey, productListAsJson);
        }
    }
}
