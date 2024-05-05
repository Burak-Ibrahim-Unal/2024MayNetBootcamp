using BootcampApi.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Immutable;
using System.Net.Mail;

namespace BootcampApi.Models
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository = new();

        public ResponseModelDto<ImmutableList<ProductDto>> GetProductsWithKdv()
        {
            #region Eski
            // 1.yol
            //var newProductList = new List<Product>();

            //foreach (var product in _productRepository.GetProducts())
            //{
            //    var newProduct = new Product
            //    {
            //        Id = product.Id,
            //        Name = product.Name,
            //        Price = product.Price * 1.2m,
            //    };
            //    newProductList.Add(newProduct);
            //}

            //return newProductList; 
            #endregion

            //2.yol
            var productList = _productRepository.GetProducts().Select(product => new ProductDto(
                product.Id,
                product.Name,
                CalculateKdv(product.Price, 1.20m),
                product.CreatedDate.ToShortDateString()
            )).ToImmutableList();


            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productList);
        }

        private decimal CalculateKdv(decimal price, decimal tax) => price * tax;

        public ProductDto? GetById(int id)
        {
            var hasProduct = _productRepository.GetById(id);

            if (hasProduct is null)
            {
                return null!;
            }
            return new ProductDto(hasProduct.Id, hasProduct.Name, hasProduct.Price, hasProduct.CreatedDate.ToShortDateString());
        }

        public ResponseModelDto<DTOs.NoContent> Delete(int id)
        {
            var hasProduct = _productRepository.GetById(id);
            var response = new ResponseModelDto<ProductDto>();

            if (hasProduct is null)
            {
                response.FailMessages = ["Başarısız.Ürün yok"];
                return ResponseModelDto<DTOs.NoContent>.Fail(["Başarısız.Ürün yok"]);
            }

            _productRepository.Delete(id);
            response.IsSuccess = true;

            return ResponseModelDto<DTOs.NoContent>.SuccessWithNoData();
        }
    }
}
