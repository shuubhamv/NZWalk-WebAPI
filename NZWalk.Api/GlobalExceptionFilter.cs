using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace NZWalk.Api
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var statusCode = HttpStatusCode.InternalServerError; // Default: 500

            if (context.Exception is ArgumentException)
                statusCode = HttpStatusCode.BadRequest; // 400
            else if (context.Exception is IOException)
                statusCode = HttpStatusCode.InternalServerError; // 500

            var response = new
            {
                Message = "An error occurred while processing your request.",
                Error = context.Exception.Message
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
