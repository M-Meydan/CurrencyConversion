using CurrencyConversion.Errors;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyConversion.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static List<ValidationError> GetValidationErrors(this ModelStateDictionary modelState)
        {
            return modelState.Keys.SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage))).ToList();
        }
    }
}
