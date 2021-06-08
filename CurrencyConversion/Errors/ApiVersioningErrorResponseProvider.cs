using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Net;

namespace CurrencyConversion.Errors
{
    public class ApiVersioningErrorResponseProvider : DefaultErrorResponseProvider
    {
        // note: in Web API the response type is HttpResponseMessage
        public override IActionResult CreateResponse(ErrorResponseContext context)
        {
            ObjectResult response;

            switch (context.ErrorCode)
            {
                case "UnsupportedApiVersion":
                    var error = new APIError(context.ErrorCode);
                    error.AddError("ApiVersion", "The requested URI cannot be mapped to a resource.");
                    response = new ObjectResult(error);
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    response = new ObjectResult(context.Message);
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
            }

            return response;
        }
    }
}
