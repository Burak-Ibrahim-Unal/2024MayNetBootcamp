﻿using Bootcamp.Clean.Api.Filters;
using Bootcamp.Clean.ApplicationService.ProductService;
using Bootcamp.Clean.ApplicationService.ProductService.DTOs;
using Bootcamp.Clean.ApplicationService.ProductService.Helpers;
using Bootcamp.Clean.ApplicationService.ProductService.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bootcamp.Clean.Api.Controllers
{
    public class ProductsController(ProductService _productService) : CustomBaseController
    {
        //baseUrl/api/products
        [HttpGet]
        public async Task<IActionResult> GetAll([FromServices] PriceCalculator priceCalculator)
        {
            return Ok(await _productService.GetAllWithCalculatedTax(priceCalculator));
        }

        [HttpGet("page/{page:int}/pagesize/{pageSize:max(50)}")]
        public async Task<IActionResult> GetAllByPage(int page, int pageSize,[FromServices] PriceCalculator priceCalculator)
        {
            return CreateActionResult(
                await _productService.GetAllByPageWithCalculatedTax(priceCalculator, page, pageSize));
        }

        // complex type => class,record,struct => request body as Json
        // simple type => int,string,decimal => query string by default / route data

        [ServiceFilter(typeof(NotFoundFilter))]
        [MyResourceFilter]
        [MyActionFilter]
        [MyResultFilter]
        [HttpGet("{productId:Guid}")]
        public async Task<IActionResult> GetById(Guid productId)
        {
            return CreateActionResult(await _productService.GetById(productId));
        }

        //[ServiceFilter(typeof(NotFoundFilter))]
        //[MyResourceFilter]
        //[MyActionFilter]
        //[MyResultFilter]
        //[HttpGet("{productId:int}")]
        //public async Task<IActionResult> GetById(int productId)
        //{
        //    return CreateActionResult(await _productService.GetById(productId));
        //}

        [SendSmsWhenExceptionFilter]
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateRequestDto request)
        {
            var result = await _productService.Create(request);

            return CreateActionResult(result, nameof(GetById), new { productId = result.Data });
        }

        [ServiceFilter(typeof(NotFoundFilter))]
        [HttpDelete("{productId:Guid}")]
        public async Task<IActionResult> Delete(Guid productId)
        {
            return CreateActionResult(await _productService.Delete(productId));
        }

        // PUT localhost/api/products/10
        [ServiceFilter(typeof(NotFoundFilter))]
        [HttpPut("{productId:Guid}")]
        public async Task<IActionResult> Update(Guid productId, ProductUpdateRequestDto request)
        {
            return CreateActionResult(await _productService.Update(productId, request));
        }

        #region Old
        //// PUT api/products   
        //[HttpPut]
        //public IActionResult Update2(ProductUpdateRequestDto request)
        //{
        //    _productService.Update(request);

        //    return NoContent();
        //} 
        #endregion

        [HttpPut("UpdateProductName")]
        public async Task<IActionResult> UpdateProductName(ProductNameUpdateRequestDto request)
        {
            return CreateActionResult(await _productService.UpdateProductName(request.Id, request.Name));
        }
    }
}
