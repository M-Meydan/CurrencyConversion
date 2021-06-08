using CurrencyConversion.Models;

namespace CurrencyConversion.Services
{
    public interface ICurrencyConverter
    {
        PriceResponse Convert(double currencyRate, string toCurrency, double amount);
    }

    public class CurrencyConverter : ICurrencyConverter
    {
        public CurrencyConverter(){}

        public PriceResponse Convert(double currencyRate, string toCurrency, double amount)
        {
            var convertedAmount = amount * currencyRate;
            return new PriceResponse(convertedAmount, toCurrency);
        }
    }
}
