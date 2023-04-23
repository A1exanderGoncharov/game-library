using BLL.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Extensions
{
    public class ExceptionHandlerMiddleware
    {
        readonly RequestDelegate _next;
        readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ElementNotFoundException enfEx)
            {
                _logger.LogError($"A new not found exception has been thrown: {enfEx}");

                await HandleExceptionAsync(httpContext, enfEx);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Something went wrong: {ex}");

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = exception switch
            {
                ElementNotFoundException => StatusCodes.Status404NotFound,

                _ => StatusCodes.Status500InternalServerError
            };

            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                int statusCode = httpContext.Response.StatusCode;
                string message = "Oh dear. Are you lost?";

                httpContext.Response.Redirect($"/Home/ErrorPage/?statusCode={statusCode}&message={message}");
            }

            await httpContext.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = $"Internal Server Error. {exception.Message}"
            }.ToString());
        }
    }
}
