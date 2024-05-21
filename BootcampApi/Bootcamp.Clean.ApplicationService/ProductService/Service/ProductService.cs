using AutoMapper;
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
    public class ProductService(IProductRepository _productRepository, IMapper mapper)
    {
        public async Task<ResponseModelDto<int>> Create(ProductCreateRequestDto request)
        {
            var newProduct = new Product
            {
                Name = request.Name.Trim(),
                Price = request.Price,
                Stock = 10,
                Barcode = Guid.NewGuid().ToString(),
                Created = DateTime.Now
            };

            var mappedProduct = mapper.Map<ProductCreateRequestDto>(newProduct);
            await _productRepository.Create(mappedProduct);

            return ResponseModelDto<int>.Success(newProduct.Id, HttpStatusCode.Created);
        }

        public async Task<ResponseModelDto<NoContent>> Delete(int id)
        {
            await _productRepository.Delete(id);

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllByPageWithCalculatedTax(
            PriceCalculator priceCalculator, int page, int pageSize)
        {
            var productsList = await _productRepository.GetAllByPageWithCalculatedTax(priceCalculator, page, pageSize);


            var productListAsDto = mapper.Map<List<ProductDto>>(productsList);

            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productListAsDto.ToImmutableList());
        }

        public async Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllWithCalculatedTax(
            PriceCalculator priceCalculator)
        {
            var productList = await _productRepository.GetAllWithCalculatedTax(priceCalculator);

            var productListAsDto = mapper.Map<List<ProductDto>>(productList);


            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productListAsDto.ToImmutableList());
        }

        public async Task<ResponseModelDto<ProductDto?>> GetByIdWithCalculatedTax(int id,
            PriceCalculator priceCalculator)
        {
            var hasProduct = await _productRepository.GetByIdWithCalculatedTax(id, priceCalculator);

            //if (hasProduct is null)
            //{
            //    return ResponseModelDto<ProductDto?>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);
            //}


            var productAsDto = mapper.Map<ProductDto>(hasProduct);

            return ResponseModelDto<ProductDto?>.Success(productAsDto);
        }

        public async Task<ResponseModelDto<NoContent>> Update(int productId, ProductUpdateRequestDto request)
        {
            var hasProduct = await _productRepository.Update(productId, request);

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<NoContent>> UpdateProductName(int id, string name)
        {
            await _productRepository.UpdateProductName(id, name);

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}
