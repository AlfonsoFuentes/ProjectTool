using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace Client.Infrastructure.Services
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T request);

    }
    public class HttpClientService : IHttpClientService
    {
        private HttpClient Http;
        private AsyncRetryPolicy<HttpResponseMessage> RetryPolicy;
        //ILogger Logger = null!;
        public HttpClientService(IHttpClientFactory httpClientFactory/*, ILogger logger*/)
        {
            //Logger = logger;
            Http = httpClientFactory.CreateClient("Auth");
            RetryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(retryCount: 3,
                Intent => TimeSpan.FromSeconds(1 * Intent),
                onRetry: (exception, time, context) =>
                {
                    //Logger.LogInformation("----Retry Communication");
                    //Logger.LogInformation($"Error time {DateTime.Now}");
                });

        }
        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var HttpResponse = await RetryPolicy.ExecuteAsync(
                async () =>
                {
                    var httpReponse = await Http.GetAsync(url);
                    return httpReponse;
                });
            HttpResponse.EnsureSuccessStatusCode();
            return HttpResponse;

        }
        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T request)
        {
            HttpResponseMessage result = new();
            try
            {
                var HttpResponse = await RetryPolicy.ExecuteAsync(
                       async () =>
                       {
                           var httpresult = await Http.PostAsJsonAsync(url, request);
                           return httpresult;
                       });
                HttpResponse.EnsureSuccessStatusCode();
                return HttpResponse;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;

        }


    }
}
