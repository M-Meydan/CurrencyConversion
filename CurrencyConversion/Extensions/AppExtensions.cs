using CurrencyConversion.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CurrencyConversion.Extensions
{
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Prices API V1");
            });
        }
        public static void UseMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
