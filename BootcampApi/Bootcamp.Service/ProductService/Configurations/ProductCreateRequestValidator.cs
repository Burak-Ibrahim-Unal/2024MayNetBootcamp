using Bootcamp.Repository.Repositories.ProductRepositories;
using Bootcamp.Service.ProductService.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.Service.ProductService.Configurations
{
    public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequestDto>
    {
        public ProductCreateRequestValidator(ISyncProductRepository _productRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull().WithMessage("{PropertyName} is required")
                .Length(5, 10).WithMessage("{PropertyName} must be between 5 and 10 characters")
                .Must(productName => ExistProductName(_productRepository, productName))
                .WithMessage("Product name already exists.");


            RuleFor(x => x.Price)
                .InclusiveBetween(1, 1000).WithMessage("Fiyat alanı 1 ile 100 arasında olmalıdır.");

            //RuleFor(x => x.IdentityNo).Length(11).WithMessage("TC numarası 11 haneli olmalıdır.").Must(CheckIdentityNo)
            //    .WithMessage("Tc numarası hatalıdı  çl5şt6yçöm3w049u8y76666yy6lhr.");
        }


        public bool ExistProductName(ISyncProductRepository _productRepository, string name)
        {
            return !_productRepository.IsExists(name);

            //var hasProduct = ;


            //return !hasProduct;


            //if (hasProduct)
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
        }

        public static bool CheckIdentityNo(string identityNo)
        {
            //Action delegate => void
            //Predicate delegate => bool
            // Func delegate = dynamic


            // business validation
            return true;
        }
    }
}
