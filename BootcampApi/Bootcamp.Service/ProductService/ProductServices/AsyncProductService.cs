using AutoMapper;
using Bootcamp.Repository.Repositories;
using Bootcamp.Repository.Repositories.ProductRepositories;
using Bootcamp.Service.ProductService.DTOs;
using Bootcamp.Service.ProductService.Helpers;
using Bootcamp.Service.SharedDto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Service.ProductService.ProductServices
{
    public class AsyncProductService(IAsyncProductRepository _productRepository, IUnitOfWork unitOfWork, IMapper mapper)
           : IAsyncProductService
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

            await _productRepository.Create(newProduct);
            await unitOfWork.CommitAsync();

            return ResponseModelDto<int>.Success(newProduct.Id, HttpStatusCode.Created);
        }

        public async Task<ResponseModelDto<NoContent>> Delete(int id)
        {
            await _productRepository.Delete(id);
            await unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllByPageWithCalculatedTax(
            PriceCalculator priceCalculator, int page, int pageSize)
        {
            var productsList = await _productRepository.GetAllByPage(page, pageSize);


            var productListAsDto = mapper.Map<List<ProductDto>>(productsList);

            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productListAsDto.ToImmutableList());
        }

        public async Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllWithCalculatedTax(
            PriceCalculator priceCalculator)
        {
            var productList = await _productRepository.GetAll();

            var productListAsDto = mapper.Map<List<ProductDto>>(productList);


            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productListAsDto.ToImmutableList());
        }

        public async Task<ResponseModelDto<ProductDto?>> GetByIdWithCalculatedTax(int id,
            PriceCalculator priceCalculator)
        {
            var hasProduct = await _productRepository.GetById(id);

            //if (hasProduct is null)
            //{
            //    return ResponseModelDto<ProductDto?>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);
            //}


            var productAsDto = mapper.Map<ProductDto>(hasProduct);

            return ResponseModelDto<ProductDto?>.Success(productAsDto);
        }

        public async Task<ResponseModelDto<NoContent>> Update(int productId, ProductUpdateRequestDto request)
        {
            var hasProduct = await _productRepository.GetById(productId);

            //if (hasProduct is null)
            //{
            //    return ResponseModelDto<NoContent>.Fail("Güncellenmeye çalışılan ürün bulunamadı.",
            //        HttpStatusCode.NotFound);
            //}


            hasProduct.Name = request.Name;
            hasProduct.Price = request.Price;


            await _productRepository.Update(hasProduct);


            await unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<NoContent>> UpdateProductName(int id, string name)
        {
            await _productRepository.UpdateProductName(name, id);

            await unitOfWork.CommitAsync();

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }


        public void TryCatchExample(string price)
        {
            try
            {
            }
            catch (Exception e)
            {
                throw;
                //  throw new Exception(e.Message);
            }


            if (decimal.TryParse(price, out decimal newPrice))
            {
            }
            else
            {
            }
        }
    }
}
