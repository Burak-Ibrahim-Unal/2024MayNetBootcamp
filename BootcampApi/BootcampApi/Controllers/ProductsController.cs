using Bootcamp.Service.ProductService.DTOs;
using Bootcamp.Service.ProductService.Helpers;
using Bootcamp.Service.ProductService.ProductServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BootcampApi.Controllers
{
    public class ProductsController : CustomBaseController
    {
        private readonly IAsyncProductService _productService;

        public ProductsController(IAsyncProductService productService)
        {
            _productService = productService;
        }

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

        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetById(int productId, [FromServices] PriceCalculator priceCalculator)
        {
            return CreateActionResult(await _productService.GetByIdWithCalculatedTax(productId, priceCalculator));
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateRequestDto request)
        {
            //throw new Exception("db'ye gidemedi");
            var result = await _productService.Create(request);

            return CreateActionResult(result, nameof(GetById), new { productId = result.Data });
        }

        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> Delete(int productId)
        {
            return CreateActionResult(await _productService.Delete(productId));
        }

        // PUT localhost/api/products/10
        [HttpPut("{productId:int}")]
        public async Task<IActionResult> Update(int productId, ProductUpdateRequestDto request)
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
