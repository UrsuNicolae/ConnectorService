using ConnectorService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ConnectorService.Filters
{
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var result = new ObjectResult(new ErrorModel()
            {
                success = false,
                error = context.Exception.Message
            });
            context.HttpContext.Response.ContentType = "application/json";
            context.Result = result;
        }
    }
}
