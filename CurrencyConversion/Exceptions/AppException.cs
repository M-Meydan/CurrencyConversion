using CurrencyConversion.Errors;
using System;
using System.Collections.Generic;

namespace CurrencyConversion.Exceptions
{
    public class AppException : Exception
    {
        public int StatusCode { get; set; }

        public IList<ValidationError> Errors { get; set; }

        public AppException(string message, int statusCode = 400, IList<ValidationError> errors = null) :
            base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }

        public void AddError(string field, string message)
        {
            Errors = Errors ?? new List<ValidationError>();
            Errors.Add(new ValidationError(field, message));
        }
    }
}
