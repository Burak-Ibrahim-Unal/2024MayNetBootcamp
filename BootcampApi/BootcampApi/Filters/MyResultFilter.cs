using Bootcamp.Service.ProductService.DTOs;
using Bootcamp.Service.SharedDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BootcampApi.Filters
{
    public class MyResultFilter : Attribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            var responseBody = (context.Result as ObjectResult).Value;

            if (responseBody is ResponseModelDto<ProductDto> response)
            {
                // loglama
            }


            Console.WriteLine("OnResultExecuting");
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine("OnResultExecuted");
        }
    }
}
