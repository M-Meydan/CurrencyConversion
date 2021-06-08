using CurrencyConversion.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CurrencyConversion.Attributes
{
    /// <summary>
    /// Validates the request model
    /// </summary>
    public class ValidateModelAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorResponse = new APIError("Validation errors." ,context.ModelState);

                context.Result = new ObjectResult(errorResponse) { StatusCode = errorResponse.StatusCode };
            }
        }
    }
}
