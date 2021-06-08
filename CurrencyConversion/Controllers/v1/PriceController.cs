using CurrencyConversion.Errors;
using CurrencyConversion.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using CurrencyConversion.Services;

namespace CurrencyConversion.Controllers.v1
{
    [Route("api/[controller]")]
    public class PriceController : ControllerBase
    {
        readonly ILogger<PriceController> _logger;
        readonly ICurrencyConverter _currencyConverter;
        readonly IExchangeRateAPIService _exchangeRateAPIService;
        readonly IExchangeRateValidator _exchangeRateValidator;
        public PriceController(ICurrencyConverter currencyConverter, 
            IExchangeRateAPIService exchangeRateAPIService,
            IExchangeRateValidator exchangeRateValidator,
            ILogger<PriceController> logger)
        {
            _logger = logger; 
            _currencyConverter = currencyConverter;
            _exchangeRateAPIService = exchangeRateAPIService;
            _exchangeRateValidator = exchangeRateValidator;
        }

        /// <remarks>
        /// Sample request:
        ///     GET api/Prices?amount=1&from=gbp&to=gbp
        /// </remarks>
        [HttpGet]
        [SwaggerOperation( Summary = "Converts the amount provided to the target currency.", Description = "Returns price in the targe currency.")]
        [ProducesResponseType(typeof(PriceResponse), 200)]
        [ProducesResponseType(typeof(APIError),400)]
        [Produces("application/json")]
        public async Task<PriceResponse> ConvertAsync([FromQuery] PriceRequest request)
        {
            var exchangeRate = await _exchangeRateAPIService.GetAsync(request.From);
            
            await _exchangeRateValidator.ValidateAsync(request.To, exchangeRate);

           return _currencyConverter.Convert(exchangeRate.GetRateValue(request.To), request.To, request.Amount);
        }
    }
}
