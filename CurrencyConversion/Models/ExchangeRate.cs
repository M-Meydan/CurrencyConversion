using CurrencyConversion.Errors;
using CurrencyConversion.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyConversion.Models
{
    public class Rate : Dictionary<string, object>{}

    public class ExchangeRate
    {
        [JsonPropertyName("base")]
        public string Base { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("time_last_updated")]
        public int TimeLastUpdated { get; set; }

        [JsonPropertyName("rates")]
        public Rate Rates{ get; set; }

        /// <summary>
        /// Returns currency exchange rate
        /// </summary>
        public double GetRateValue(string toCurrency)
        {
            return double.Parse(Rates[toCurrency].ToString());
        }
    }

    public interface IExchangeRateValidator : IValidator<ExchangeRate>
    {
        Task ValidateAsync(string toCurrency, ExchangeRate exchangeRates);
    }
    public class ExchangeRateValidator : AbstractValidator<ExchangeRate>, IExchangeRateValidator
    {
        public ExchangeRateValidator()
        {
            RuleFor(p => p.Base).NotNull().NotEmpty();
            RuleFor(p => p.Date).NotNull().NotEmpty(); 
            RuleFor(p => p.TimeLastUpdated).GreaterThan(0);

            RuleFor(p => p.Rates).NotNull().Must(list => list.Count > 0);
            RuleFor(p => p.Rates).Custom((dictionary, context) =>
            {
                //To currency should be exist and be greater than 0
                if (!context.RootContextData.TryGetValue("ToCurrency", out object toCurrency)
                        || !dictionary.TryGetValue(toCurrency.ToString(), out object toCurrencyObject)
                        || !double.TryParse(toCurrencyObject.ToString(), out double toCurrencyValue)
                        || toCurrencyValue <= 0)
                {

                    context.AddFailure(new ValidationFailure("To", $"Invalid currency '{toCurrency}' provided!"));
                }
            });
        }

        public async Task ValidateAsync(string toCurrency, ExchangeRate exchangeRates)
        {
            var validationContext = new ValidationContext<ExchangeRate>(exchangeRates);
            validationContext.RootContextData["ToCurrency"] = toCurrency;
            var result = await this.ValidateAsync(validationContext);

            if (!result.IsValid)
            {
                throw new AppException("Exchange rate response is not valid",
                    errors: result.Errors.Select(error => new ValidationError(error.PropertyName, error.ErrorMessage)).ToList());
            }
        }

        protected override bool PreValidate(ValidationContext<ExchangeRate> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure("", "Request data was not provided!"));
                return false;
            }
            return true;
        }
    }
}
