using Microsoft.AspNetCore.Mvc.Filters;

namespace Bootcamp.Clean.Api.Filters
{
    public class MyResourceFilter : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var actionName = context.RouteData.Values["action"];

            Console.WriteLine("OnResourceExecuting");
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine("OnResourceExecuted");
        }
    }
}
