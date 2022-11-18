using ConnectorService.Models;
using ConnectorService.Policies;
using ConnectorService.Queries;
using MediatR;
using Newtonsoft.Json;

namespace ConnectorService.Handlers
{
    public class FormatQueryHandler : IRequestHandler<StringQuery, string>
    {
        private readonly RetryPolicy _retryPolicy;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public FormatQueryHandler(
            RetryPolicy retryPolicy,
            IHttpClientFactory clientFactory,
            IConfiguration configuration)
        {
            _retryPolicy = retryPolicy;
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        public async Task<string> Handle(StringQuery request, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient();
            var queryParams = new Dictionary<string, string>
            {
                { "sql", request.QueryString },
                { "reindent", "1" }
            };
            var encodedContent = new FormUrlEncodedContent(queryParams);
            var response = await _retryPolicy.LinearHttpRetry.ExecuteAsync(()
                => client.PostAsync(_configuration["SqlFormatURL"], encodedContent, cancellationToken));
            string strResponse = await response.Content.ReadAsStringAsync(cancellationToken);
            var deserializedResult = JsonConvert.DeserializeObject<SqlFormatResult>(strResponse);
            return deserializedResult.Result;
        }
    }
}
