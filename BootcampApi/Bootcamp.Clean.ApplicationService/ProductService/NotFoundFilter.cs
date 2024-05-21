using Bootcamp.Clean.ApplicationService.ProductService.DTOs;
using Bootcamp.Clean.ApplicationService.ProductService.Service;
using Bootcamp.Clean.ApplicationService.SharedDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bootcamp.Clean.ApplicationService.ProductService
{
    public class NotFoundFilter(IProductRepository productRepository) : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // how to get action name
            // fast fail
            // guard clauses
            var actionName = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;


            var productIdFromAction = context.ActionArguments.Values.First()!;
            int productId = 0;

            if (actionName == "UpdateProductName" &&
                productIdFromAction is ProductNameUpdateRequestDto productNameUpdateRequestDto)
            {
                productId = productNameUpdateRequestDto.Id;
            }


            if (productId == 0 && !int.TryParse(productIdFromAction.ToString(), out productId))
            {
                return;
            }

            var hasProduct = productRepository.HasExist(productId).Result;

            if (!hasProduct)
            {
                var errorMessage = $"There is no product with id: {productId}";

                var responseModel = ResponseModelDto<NoContent>.Fail(errorMessage);
                context.Result = new NotFoundObjectResult(responseModel);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
