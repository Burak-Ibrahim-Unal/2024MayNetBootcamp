using AutoMapper;
using Bootcamp.Clean.ApplicationService.ProductService.DTOs;
using Bootcamp.Clean.ApplicationService.ProductService.Helpers;
using Bootcamp.Clean.ApplicationService.ProductService.Service;
using Bootcamp.Clean.ApplicationService.SharedDto;
using Bootcamp.Clean.Domain.Entities;
using Bootcamp.Clean.Repository.Context;
using Bootcamp.Clean.Repository.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public class ProductRepository(AppDbContext context) : IProductRepository
    {
        public async Task<Guid> Create(Product product)
        {
            await context.Products.AddAsync(product);

            return product.Id;
        }

        public async Task Delete(Guid id)
        {
            var removeToProduct = await context.Products.FindAsync(id);

            await Task.Run(() => context.Products.Remove(removeToProduct!));
            //context.Products.Remove(removeToProduct!);
        }

        public async Task<List<Product>> GetAllByPageWithCalculatedTax(PriceCalculator priceCalculator, int page, int pageSize)
        {
            var productsList = await context.Products.AsQueryable().AsNoTracking().Skip(page - 1).Take(pageSize).ToListAsync();

            return productsList;
        }

        public async Task<List<Product>> GetAllWithCalculatedTax(PriceCalculator priceCalculator)
        {
            var productList = await context.Products.AsQueryable().AsNoTracking().ToListAsync();

            return productList;
        }

        public async Task<Product?> GetById(Guid id)
        {
            var product = await context.Products.FindAsync(id);

            return product;

        }

        public async Task<Product?> GetByIdWithCalculatedTax(Guid id, PriceCalculator priceCalculator)
        {
            var product = await context.Products.FindAsync(id);

            return product;
        }

        public async Task<bool> HasExist(Guid id)
        {
            return await context.Products.AnyAsync(x => x.Id == id);
        }

        public async Task Update(Guid productId, ProductUpdateRequestDto request)
        {
            var product = await context.Products.FindAsync(productId);

            product!.Name = request.Name;
            product.Price = request.Price;

            await Task.Run(() => context.Products.Update(product));
            //context.Products.Update(hasProduct);
        }

        public async Task UpdateProductName(Guid id, string name)
        {
            var product = await context.Products.FindAsync(id);

            product!.Name = name;
            await Task.Run(() => context.Products.Update(product));
            //context.Products.Update(hasProduct);
        }
    }
}
