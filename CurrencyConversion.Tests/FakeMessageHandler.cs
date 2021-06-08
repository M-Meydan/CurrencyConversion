using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace CurrencyConversion.Tests
{
    public class ExchangeRateServiceTests
    {
        ///Helper class for mocking the MessageHandler dependency of HttpClient
        public abstract class FakeMessageHandler : HttpMessageHandler
        {
            protected sealed override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                return DoSendAsync(request);
            }

            public abstract Task<HttpResponseMessage> DoSendAsync(HttpRequestMessage request);
        }
    }
}