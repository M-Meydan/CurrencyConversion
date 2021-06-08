using CurrencyConversion.Attributes;
using CurrencyConversion.Extensions;
using CurrencyConversion.Models;
using CurrencyConversion.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyConversion
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        { 
            services.AddControllers((options =>{ options.Filters.Add<ValidateModelAttribute>(); }))
                .ConfigureApiBehaviorOptions((options => { options.SuppressModelStateInvalidFilter = true; }))
                .AddJsonOptions(options => { options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null; //sets pascal case
                });

            services.AddLogging();
            services.AddFluentValidationExtension();
            services.AddSwaggerExtension();
            services.AddHttpClient();

            services.AddTransient<IExchangeRateAPIService, ExchangeRateAPIService>();
            services.AddTransient<IExchangeRateValidator, ExchangeRateValidator>();
            services.AddTransient<ICurrencyConverter, CurrencyConverter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwaggerExtension();
            }

            app.UseMiddlewares();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
