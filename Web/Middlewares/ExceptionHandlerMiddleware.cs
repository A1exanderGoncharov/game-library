using BLL.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
            catch (ObjectNotFoundException onfEx)
            {
                _logger.LogError($"ObjectNotFoundException has been thrown: {onfEx}");

                HandleExceptionAsync(httpContext, onfEx);
            }
            catch (ArgumentOutOfRangeException aorE)
            {
                _logger.LogError($"ArgumentOutOfRangeException has been thrown: {aorE}");

                HandleExceptionAsync(httpContext, aorE);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal Server Error: {ex}");

                HandleExceptionAsync(httpContext, ex);
            }
        }

        private static void HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            int statusCode;
            string message;

            switch (exception)
            {
                case ObjectNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    message = "Oh dear. Are you lost?";
                    break;
                case ArgumentOutOfRangeException:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = "Oops! Our server don't understand that request.";
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "Gremlins invaded our server. Stay tuned!";
                    break;
            }

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.Redirect($"/Home/ErrorPage/?statusCode={statusCode}&message={message}");
        }

    }
}
