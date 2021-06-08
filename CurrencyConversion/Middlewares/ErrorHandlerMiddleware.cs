using CurrencyConversion.Errors;
using CurrencyConversion.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyConversion.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        readonly RequestDelegate _next;
        readonly ILogger _logger;
        public ErrorHandlerMiddleware(RequestDelegate next, ILoggerFactory logger){ _next = next; _logger= logger.CreateLogger("ErrorHandlerMiddleware"); } //ILoggerManager logger

        public async Task Invoke(HttpContext context, IWebHostEnvironment env)
        {
            try
            { await _next(context); }
            catch (Exception ex) { await HandleExceptionAsync(context, env, ex); }
        }

        private Task HandleExceptionAsync(HttpContext context, IWebHostEnvironment env, Exception exception)
        {
            //var apiResponse = new APIResponse("Server error occurred!", new APIError(exception, env.IsDevelopment()), HttpStatusCode.InternalServerError);

            var apiResponse = new APIError("Request failed with errors!",exception, env.IsDevelopment());
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = apiResponse.StatusCode;

            var json = JsonSerializer.Serialize(apiResponse);
            //_logger.LogInformation(json);

            return context.Response.WriteAsync(json);
        }
    }
}
