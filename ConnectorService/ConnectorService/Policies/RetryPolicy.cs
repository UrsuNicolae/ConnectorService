using System.Net;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace ConnectorService.Policies
{
    public class RetryPolicy
    {
        public AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetry { get; }

        public RetryPolicy()
        {
            LinearHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                    res => !res.IsSuccessStatusCode)
                .RetryAsync(3); ;
        }

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                    retryAttempt)));
        }
    }
}
