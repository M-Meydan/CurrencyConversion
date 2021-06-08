using CurrencyConversion.Exceptions;
using CurrencyConversion.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyConversion.Errors
{
    /// <summary>
    /// ApiError contains error details
    /// </summary>
    public class APIError
    {
        public int StatusCode { get; set; } = 400;
        public string ErrorMessage { get; set; }

        public IList<ValidationError> Errors { get; set; }

        public APIError(string message) { ErrorMessage = message; }

        public APIError(string message, ModelStateDictionary modelState)
        {
            if (modelState != null && modelState.Any(m => m.Value.Errors.Count > 0))
            {
                ErrorMessage = message; 
                Errors = modelState.GetValidationErrors();
            }
        }

        public APIError(string message, Exception exception, bool isDevelopment)
        {
            if (exception is AppException)
            {
                var appException = (exception as AppException);
                Errors = appException.Errors;
                ErrorMessage = message;
                StatusCode = appException.StatusCode;
            }
            else
            {
                ErrorMessage = $"{message}{Environment.NewLine} { (isDevelopment ? exception.ToString() : exception.Message)}";
                StatusCode = 500; // internal system error
            }
        }

        public void AddError(string field, string message)
        {
            Errors = Errors ?? new List<ValidationError>();
            Errors.Add(new ValidationError(field,message));
        }
    }
}
