using CurrencyConversion.Errors;
using CurrencyConversion.Exceptions;
using CurrencyConversion.Models;
using FluentValidation;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyConversion.Services
{
    public interface IExchangeRateAPIService
    {
        Task<ExchangeRate> GetAsync(string fromCurrency);
    }

    public class ExchangeRateAPIService : IExchangeRateAPIService
    {
        const string BaseUrl = "https://trainlinerecruitment.github.io/exchangerates/api/latest/{0}.json";
        readonly IHttpClientFactory _httpClientFactory;

        public ExchangeRateAPIService(IHttpClientFactory httpClientFactory) { _httpClientFactory = httpClientFactory; }

        public async Task<ExchangeRate> GetAsync(string fromCurrency)
        {
            var url = string.Format(BaseUrl, fromCurrency);
            string jsonResponse = string.Empty;

            var client = _httpClientFactory.CreateClient("apiClient");
            try
            {
                jsonResponse = await client.GetStringAsync(url);
               return JsonSerializer.Deserialize<ExchangeRate>(jsonResponse, null);
            }
            catch (Exception exception)
            {
                var appException = new AppException("Cannot retrieve exchange rate from API.");
                appException.AddError(exception.Message, url);

                throw appException;
            }
        }
    }
}
