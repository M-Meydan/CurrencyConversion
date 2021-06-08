using CurrencyConversion.Models;
using FluentAssertions;
using FluentValidation;
using NUnit.Framework;

namespace CurrencyConversion.Tests.ModelTests
{
    [TestFixture(Category = "Validator: PriceRequestValidator")]
    public class PriceRequestValidatorTests
    {
        IValidator<PriceRequest> _priceValidator;

        [SetUp]
        public void Setup(){ _priceValidator = new PriceRequestValidator(); }

        [TestCase(1, true)]
        [TestCase(5, true)]
        [TestCase(0, false)]
        [TestCase(-1, false)]
        public void Amount_should_pass_when_value_greater_than_0(double amount, bool validationResult)
        {
            var expectedPriceRequest = new PriceRequest() { Amount = amount, From = "USD", To = "GBP" };

            //Act
            var result = _priceValidator.Validate(expectedPriceRequest);

            //Assert
            result.IsValid.Should().Be(validationResult);
        }

        [TestCase("usd", true)]
        [TestCase("asdfgh", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        public void From_should_pass_when_value_provided(string from, bool validationResult)
        {
            var expectedPriceRequest = new PriceRequest() { From = from, To = "GBP", Amount = 1};

            //Act
            var result = _priceValidator.Validate(expectedPriceRequest);

            //Assert
            result.IsValid.Should().Be(validationResult);
        }

        [TestCase("usd", true)]
        [TestCase("asdfgh", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        public void To_should_pass_when_value_provided(string to, bool validationResult)
        {
            var expectedPriceRequest = new PriceRequest() {  To = to, From = "GBP", Amount = 1 };

            //Act
            var result = _priceValidator.Validate(expectedPriceRequest);

            //Assert
            result.IsValid.Should().Be(validationResult);
        }
    }
}