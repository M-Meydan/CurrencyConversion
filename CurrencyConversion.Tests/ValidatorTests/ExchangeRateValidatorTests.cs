using CurrencyConversion.Exceptions;
using CurrencyConversion.Models;
using FluentAssertions;
using NUnit.Framework;

namespace CurrencyConversion.Tests.ModelTests
{
    [TestFixture(Category = "Validator: ExchangeRateValidator")]
    public class ExchangeRateValidatorTests
    {
        IExchangeRateValidator  _exchangeRateValidator;

        [SetUp]
        public void Setup(){ _exchangeRateValidator = new ExchangeRateValidator(); }

        [TestCase("USD", true)]
        [TestCase("GBP", true)]
        public void Base_should_be_exist(string baseCurrency, bool validationResult)
        {
            var expectedExchangeRate = new ExchangeRate()
            {
                Base = baseCurrency,
                Date = "2021-04-07",
                TimeLastUpdated = 1617753602,
                Rates = new Rate() { { "USD", 1 }, { "EUR", 0.844712 }, { "GBP", 0.722077 } }
            };

            //Act
            FluentActions.Invoking(async () => await _exchangeRateValidator.ValidateAsync(baseCurrency, expectedExchangeRate))
                .Should().NotThrow();
        }

        [Test]
        public void Date_should_be_exist()
        {
            var expectedExchangeRate = new ExchangeRate()
            {
                Base = "USD",
                Date = "2021-04-07",
                TimeLastUpdated = 1617753602,
                Rates = new Rate() { { "USD", 1 }, { "EUR", 0.844712 }, { "GBP", 0.722077 } }
            };

            //Act
            FluentActions.Invoking(async () => await _exchangeRateValidator.ValidateAsync("USD", expectedExchangeRate))
                .Should().NotThrow();
        }

        [Test]
        public void TimeLastUpdated_should_be_exist()
        {
            var expectedExchangeRate = new ExchangeRate()
            {
                Base = "USD",
                Date = "2021-04-07",
                TimeLastUpdated = 1617753602,
                Rates = new Rate() { { "USD", 1 }, { "EUR", 0.844712 }, { "GBP", 0.722077 } }
            };

            //Act
            FluentActions.Invoking(async () => await _exchangeRateValidator.ValidateAsync("USD", expectedExchangeRate))
                .Should().NotThrow();
        }

        [Test]
        public void Rates_should_contain_curreny_rates()
        {
            var expectedExchangeRate = new ExchangeRate()
            {
                Base = "USD",
                Date = "2021-04-07",
                TimeLastUpdated = 1617753602,
                Rates = new Rate() { { "USD", 1 }, { "EUR", 0.844712 }, { "GBP", 0.722077 } }
            };

            //Act
            FluentActions.Invoking(async () => await _exchangeRateValidator.ValidateAsync("USD", expectedExchangeRate))
                .Should().NotThrow();
        }

        [TestCase("USSDD")]
        public void Rates_should_fail_when_to_curreny_not_exist(string toCurrency)
        {
            var expectedExchangeRate = new ExchangeRate()
            {
                Base = "USD",
                Date = "2021-04-07",
                TimeLastUpdated = 1617753602,
                Rates = new Rate() { { "USD", 1 }, { "EUR", 0.844712 }, { "GBP", 0.722077 } }
            };

            //Act
            FluentActions.Invoking(async () => await _exchangeRateValidator.ValidateAsync(toCurrency, expectedExchangeRate))
                .Should().Throw<AppException>();
        }

        [TestCase("USD", 0)]
        [TestCase("GBP", -1)]
        [TestCase("EUR", -1.1)]
        public void Rates_should_fail_when_values_lower_than_1(string toCurrency, double value)
        {
            var expectedExchangeRate = new ExchangeRate()
            {
                Base = "USD",
                Date = "2021-04-07",
                TimeLastUpdated = 1617753602,
                Rates = new Rate() { { toCurrency, value } }
            };

            //Act
            FluentActions.Invoking(async () => await _exchangeRateValidator.ValidateAsync(toCurrency, expectedExchangeRate))
                .Should().Throw<AppException>();
        }
    }
}