using AutoMapper;
using Bootcamp.Clean.ApplicationService.Interfaces;
using Bootcamp.Clean.ApplicationService.ProductService.DTOs;
using Bootcamp.Clean.ApplicationService.ProductService.Helpers;
using Bootcamp.Clean.ApplicationService.SharedDto;
using Bootcamp.Clean.Domain.Entities;
using Bootcamp.Clean.Repository.Context;
using Bootcamp.Clean.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bootcamp.Clean.Repository.Repositories.ProductRepositories
{
    public class ProductRepository(AppDbContext context, IUnitOfWork unitOfWork, IMapper _mapper) : IProductRepository
    {
        public async Task<ResponseModelDto<Guid>> Create(Product product)
        {
            await context.Products.AddAsync(product);
            await unitOfWork.CommitAsync();

            return ResponseModelDto<Guid>.Success(product.Id, HttpStatusCode.Created);
        }

        public async Task<ResponseModelDto<NoContent>> Delete(Guid id)
        {
            var removeToProduct = await context.Products.FindAsync(id);

            await Task.Run(() => context.Products.Remove(removeToProduct!));
            //context.Products.Remove(removeToProduct!);
            await unitOfWork.CommitAsync();

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllByPageWithCalculatedTax(PriceCalculator priceCalculator, int page, int pageSize)
        {
            var productsList = await context.Products.AsQueryable().AsNoTracking().Skip(page - 1).Take(pageSize).ToListAsync();

            var productListAsDto = _mapper.Map<List<ProductDto>>(productsList);

            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productListAsDto.ToImmutableList());
        }

        public async Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllWithCalculatedTax(PriceCalculator priceCalculator)
        {
            var productList = await context.Products.AsQueryable().AsNoTracking().ToListAsync();

            var productListAsDto = _mapper.Map<List<ProductDto>>(productList);

            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productListAsDto.ToImmutableList());
        }

        public async Task<ResponseModelDto<ProductDto?>> GetById(Guid id)
        {
            var product = await context.Products.FindAsync(id);
            //if (product is null)
            //    return ResponseModelDto<ProductDto?>.Fail("Not found");

            var productAsDto = _mapper.Map<ProductDto>(product);

            return ResponseModelDto<ProductDto?>.Success(productAsDto);

        }

        public async Task<ResponseModelDto<ProductDto?>> GetByIdWithCalculatedTax(Guid id, PriceCalculator priceCalculator)
        {
            var hasProduct = await context.Products.FindAsync(id);

            var productAsDto = _mapper.Map<ProductDto>(hasProduct);

            return ResponseModelDto<ProductDto?>.Success(productAsDto);
        }

        public async Task<bool> HasExist(Guid id)
        {
            return await context.Products.AnyAsync(x => x.Id == id);
        }

        public async Task<ResponseModelDto<NoContent>> Update(Guid productId, ProductUpdateRequestDto request)
        {
            var hasProduct = await context.Products.FindAsync(productId);

            hasProduct!.Name = request.Name;
            hasProduct.Price = request.Price;

            await Task.Run(() => context.Products.Update(hasProduct));
            //context.Products.Update(hasProduct);

            await unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<NoContent>> UpdateProductName(Guid id, string name)
        {
            var hasProduct = await context.Products.FindAsync(id);

            hasProduct!.Name = name;
            await Task.Run(() => context.Products.Update(hasProduct));
            //context.Products.Update(hasProduct);

            await unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}
