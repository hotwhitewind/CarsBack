using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using TestCars.Domain;
using System.Text.Json;

namespace TestCars.Middleware
{
    public sealed class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            IWebHostEnvironment hostingEnvironment)
        {
            _next = next;
            _hostingEnvironment = hostingEnvironment;
        }


        private ILogger GetLogger(HttpContext context)
        {
            return context.RequestServices.GetRequiredService<ILogger>();
        }

        private const string ErrorOccuredContactSupportMessage =
            "An error occurred while processing your request. Please retry later or contact support@stenn.com.";
        
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException domainException)
            {
                GetLogger(context)
                    .LogInformation(domainException, "Business error on request handling: {error}", 
                        domainException.Message);

                if (!await HandleException(context, domainException,
                    domainException.Message,
                    StatusCodes.Status400BadRequest, 
                        domainException.Code, domainException.ParametersString))
                {
                    throw;
                }
            }
            catch (Exception exception)
            {
                GetLogger(context)
                    .LogError(exception, "Error on request handling: {error}", exception.Message);

                if (!await HandleException(context, exception, 
                    ErrorOccuredContactSupportMessage,
                    StatusCodes.Status500InternalServerError))
                {
                    throw;
                }
            }
        }

        private async Task<bool> HandleException(HttpContext context, Exception exception, string responseMessage, 
            int responseCode, int? errorCode = null, string[] parameters = null)
        {
            if (context.Response.HasStarted)
            {
                return false;
            }

            context.Response.Clear();
            context.Response.StatusCode = responseCode;
            context.Response.OnStarting(ResetHeaders, context.Response);

            string message;
            //if (_hostingEnvironment.)
            //{
            //    message = exception.ToString();
            //}
            //else
            //{
            //    message = responseMessage;
            //}

            message = responseMessage;
            if (errorCode.HasValue)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(
                    new { code = errorCode.Value, message, parameters }));
            }
            else
                await context.Response.WriteAsync(message, Encoding.UTF8);

            return true;
        }

        private static Task ResetHeaders(object state)
        {
            var response = (HttpResponse)state;
            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers[HeaderNames.ContentType] = "text/plain;charset=UTF-8";
            response.Headers.Remove(HeaderNames.ETag);
            return Task.CompletedTask;
        }
    }
}
