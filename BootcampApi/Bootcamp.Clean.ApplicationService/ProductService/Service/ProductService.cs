﻿using AutoMapper;
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
    public class ProductService(IProductRepository _productRepository)
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

            await _productRepository.Create(newProduct);

            return ResponseModelDto<Guid>.Success(newProduct.Id, HttpStatusCode.Created);
        }

        public async Task<ResponseModelDto<NoContent>> Delete(Guid id)
        {
            await _productRepository.Delete(id);

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<ProductDto?>> GetById(Guid id)
        {
            var product = await _productRepository.GetById(id);

            return product;
        }

        public async Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllByPageWithCalculatedTax(
            PriceCalculator priceCalculator, int page, int pageSize)
        {
            var productsList = await _productRepository.GetAllByPageWithCalculatedTax(priceCalculator, page, pageSize);

            return productsList;
        }

        public async Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllWithCalculatedTax(
            PriceCalculator priceCalculator)
        {
            var productList = await _productRepository.GetAllWithCalculatedTax(priceCalculator);

            return productList;
        }

        public async Task<ResponseModelDto<ProductDto?>> GetByIdWithCalculatedTax(Guid id,
            PriceCalculator priceCalculator)
        {
            var product = await _productRepository.GetByIdWithCalculatedTax(id, priceCalculator);

            return product;
        }

        public async Task<ResponseModelDto<NoContent>> Update(Guid productId, ProductUpdateRequestDto request)
        {
            await _productRepository.Update(productId, request);

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<NoContent>> UpdateProductName(Guid id, string name)
        {
            await _productRepository.UpdateProductName(id, name);

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}
