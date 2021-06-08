using CurrencyConversion.Exceptions;
using CurrencyConversion.Models;
using CurrencyConversion.Services;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static CurrencyConversion.Tests.ExchangeRateServiceTests;

namespace CurrencyConversion.Tests.ServiceTests
{
    [TestFixture(Category = "ExchangeRateServiceTests")]
    public partial class ExchangeRateServiceTests
    {
        IExchangeRateAPIService _exchangeRateService;

        [SetUp]
        public void Setup() {}

        [TestCase("USD")]
        [TestCase("EUR")]
        [TestCase("GBP")]
        public async Task GetExchangeRate_should_return_exchangerate(string currency)
        {
            var expectedExchangeRate = new ExchangeRate() { Base= currency, Rates = new Rate() { {"USD",1 }, { "EUR", 0.844712 }, { "GBP", 0.722077 } } };
            var httpClientMock = A.Fake<IHttpClientFactory>();
            var messageHandlerMock = A.Fake<FakeMessageHandler>();
            var client = new HttpClient(messageHandlerMock);

            A.CallTo(() => httpClientMock.CreateClient("apiClient")).Returns(client);
            A.CallTo(() => messageHandlerMock.DoSendAsync(A<HttpRequestMessage>._))
                .Returns(new HttpResponseMessage{ StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonSerializer.Serialize(expectedExchangeRate, null)) });

            //Act
             _exchangeRateService = new ExchangeRateAPIService(httpClientMock);
            var result = await _exchangeRateService.GetAsync(currency);

            //Assert
            A.CallTo(() => messageHandlerMock.DoSendAsync(A<HttpRequestMessage>._)).MustHaveHappened();
            result.Should().BeOfType<ExchangeRate>();
            result.Base.Should().Be(expectedExchangeRate.Base);
            result.Rates.Should().HaveSameCount(expectedExchangeRate.Rates);
            result.Rates.Should().ContainKey(currency);
        }

        [TestCase("")]
        [TestCase("TEUR")]
        public void GetExchangeRate_should_throw_AppException_when_currency_is_invalid(string currency)
        {
            var expectedExchangeRate = new ExchangeRate() { Base = currency, Rates = new Rate() { { "USD", 1 }, { "EUR", 0.844712 }, { "GBP", 0.722077 } } };
            var httpClientMock = A.Fake<IHttpClientFactory>();
            var messageHandlerMock = A.Fake<FakeMessageHandler>();
            var client = new HttpClient(messageHandlerMock);

            A.CallTo(() => httpClientMock.CreateClient("apiClient")).Returns(client);
            A.CallTo(() => messageHandlerMock.DoSendAsync(A<HttpRequestMessage>._)).Throws(new AppException("error"));

            //Act
            _exchangeRateService = new ExchangeRateAPIService(httpClientMock);

            //Assert
            FluentActions.Invoking(async () => await _exchangeRateService.GetAsync(currency)).Should().Throw<AppException>();
        }
    }
}