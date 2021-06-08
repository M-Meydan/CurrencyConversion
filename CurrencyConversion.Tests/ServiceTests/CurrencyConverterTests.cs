using CurrencyConversion.Models;
using CurrencyConversion.Services;
using FluentAssertions;
using NUnit.Framework;

namespace CurrencyConversion.Tests.ServiceTests
{
    [TestFixture(Category = "CurrencyConverterTests")]
    public class CurrencyConverterTests
    {
        ICurrencyConverter _currencyConverter;

        [SetUp]
        public void Setup(){ _currencyConverter = new CurrencyConverter(); }

        [TestCase(1, 1, 1)]
        [TestCase(1, 2, 2)]
        [TestCase(1.5, 5, 7.5)]
        [TestCase(1.168852, 10.10, 11.8054052)]
        public void Convert_should_calculate_amount_with_currency_rate(double currencyRate, double amount, double expectedAmount)
        {
            var currency = "GBP";
            var expectedPriceResponse = new PriceResponse(expectedAmount, currency);

            //Act
            var result = _currencyConverter.Convert(currencyRate, currency, amount);

            //Assert
            result.Should().BeOfType<PriceResponse>();
            result.Amount.Should().Be(expectedPriceResponse.Amount);
            result.Currency.Should().Be(expectedPriceResponse.Currency);
        }
    }
}