using BootcampApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BootcampApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService = new();

        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _productService.GetProductsWithKdv();
            //products[0].Name = "aaa"; // hata alacak

            return Ok(products);
        }

        [HttpGet]
        public IActionResult GetProductById(int id) 
        {
            var product = _productService.GetById(id);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _productService.Delete(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result.FailMessages);
            }

            return NoContent();
        }
    }
}
