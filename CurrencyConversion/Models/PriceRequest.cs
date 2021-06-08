using FluentValidation;
using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace CurrencyConversion.Models
{
    public class PriceRequest
    {
        string _from, _to;

        /// <summary>
        /// Amount to be converted.
        /// </summary>
        public double Amount { get; set ; }

        /// <summary>
        /// Source Currency e.g. GBP
        /// </summary>
        public string From { get => _from; set => _from = value?.ToUpper();  }

        /// <summary>
        /// Target Currency e.g. USD
        /// </summary>
        public string To { get => _to; set => _to = value?.ToUpper(); }
    }


    public class PriceRequestValidator: AbstractValidator<PriceRequest>
    {
        public PriceRequestValidator()
        {
            RuleFor(p => p.Amount).GreaterThan(0);
            RuleFor(p => p.Amount).GreaterThan(-1);
            RuleFor(p => p.From).NotNull().NotEmpty();
            RuleFor(p => p.To).NotNull().NotEmpty();
        }

        protected override bool PreValidate(ValidationContext<PriceRequest> context, ValidationResult result)
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
