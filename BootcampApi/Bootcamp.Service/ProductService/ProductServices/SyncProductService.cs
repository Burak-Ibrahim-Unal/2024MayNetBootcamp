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
    public class SyncProductService(ISyncProductRepository _productRepository, IUnitOfWork unitOfWork) : ISyncProductService
    {
        //private readonly IProductRepository _productRepository;

        //public ProductService(IProductRepository productRepository)
        //{
        //    _productRepository = productRepository;

        //}


        public ResponseModelDto<ImmutableList<ProductDto>> GetAllWithCalculatedTax(
            PriceCalculator priceCalculator)
        {
            var productList = _productRepository.GetAll().Select(product => new ProductDto(
                product.Id,
                product.Name,
                priceCalculator.CalculateKdv(product.Price, 1.20m),
                product.Created.ToShortDateString()
            )).ToImmutableList();


            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productList);
        }


        public ResponseModelDto<ImmutableList<ProductDto>> GetAllByPageWithCalculatedTax(
            PriceCalculator priceCalculator, int page, int pageSize)
        {
            var productList = _productRepository.GetAllByPage(page, pageSize).Select(product => new ProductDto(
                product.Id,
                product.Name,
                priceCalculator.CalculateKdv(product.Price, 1.20m),
                product.Created.ToShortDateString()
            )).ToImmutableList();


            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productList);
        }


        public ResponseModelDto<ProductDto?> GetByIdWithCalculatedTax(int id,
            PriceCalculator priceCalculator)
        {
            var hasProduct = _productRepository.GetById(id);

            if (hasProduct is null)
            {
                return ResponseModelDto<ProductDto?>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);
            }


            var newDto = new ProductDto(
                hasProduct.Id,
                hasProduct.Name,
                priceCalculator.CalculateKdv(hasProduct.Price, 1.20m),
                hasProduct.Created.ToShortDateString()
            );

            return ResponseModelDto<ProductDto?>.Success(newDto);
        }

        // write Add Method
        public ResponseModelDto<int> Create(ProductCreateRequestDto request)
        {
            // fast fail
            // Guard clauses

            //var hasProduct = _productRepository.IsExists(request.Name.Trim());

            //if (hasProduct)
            //{
            //    return ResponseModelDto<int>.Fail("Oluşturma çalıştığınız ürün bulunmaktadır.",
            //        HttpStatusCode.BadRequest);
            //}


            var newProduct = new Product
            {
                //Id = _productRepository.GetAll().Count + 1,
                Name = request.Name.Trim(),
                Price = request.Price,
                Stock = 10,
                Barcode = Guid.NewGuid().ToString(),
                Created = DateTime.Now
            };

            _productRepository.Create(newProduct);

            unitOfWork.Commit();
            return ResponseModelDto<int>.Success(newProduct.Id, HttpStatusCode.Created);
        }

        // write update method


        public ResponseModelDto<NoContent> UpdateProductName(int productId, string name)
        {
            var hasProduct = _productRepository.GetById(productId);

            if (hasProduct is null)
            {
                return ResponseModelDto<NoContent>.Fail("Güncellenmeye çalışılan ürün bulunamadı.",
                    HttpStatusCode.NotFound);
            }

            // productRepository.UpdateProductName(name, productId);
            unitOfWork.Commit();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }


        public ResponseModelDto<NoContent> Update(int productId, ProductUpdateRequestDto request)
        {
            var hasProduct = _productRepository.GetById(productId);

            if (hasProduct is null)
            {
                return ResponseModelDto<NoContent>.Fail("Güncellenmeye çalışılan ürün bulunamadı.",
                    HttpStatusCode.NotFound);
            }


            hasProduct.Name = request.Name;
            hasProduct.Price = request.Price;


            _productRepository.Update(hasProduct);


            unitOfWork.Commit();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }


        public ResponseModelDto<NoContent> Delete(int id)
        {
            var hasProduct = _productRepository.GetById(id);

            if (hasProduct is null)
            {
                return ResponseModelDto<NoContent>.Fail("Silinmeye çalışılan ürün bulunamadı.",
                    HttpStatusCode.NotFound);
            }


            _productRepository.Delete(id);
            unitOfWork.Commit();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}
