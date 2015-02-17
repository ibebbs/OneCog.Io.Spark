using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    public interface IRestClient
    {
        Task<Stream> Get(Uri uri);

        Task<Stream> Post(Uri uri, HttpContent content);
    }

    public class RestClient : IRestClient
    {
        private readonly HttpClient _client;
        private readonly IScheduler _requestScheduler;
        private readonly IScheduler _responseScheduler;

        public RestClient(string accessToken, IScheduler requestScheduler, IScheduler responseScheduler)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            _requestScheduler = requestScheduler ?? new EventLoopScheduler();
            _responseScheduler = responseScheduler ?? TaskPoolScheduler.Default;
        }

        public Task<Stream> Get(Uri uri)
        {
            return Observable
                .StartAsync(() => _client.GetStreamAsync(uri))
                .SubscribeOn(_requestScheduler)
                .ObserveOn(_responseScheduler)
                .ToTask();
        }

        public Task<Stream> Post(Uri uri, HttpContent content)
        {
            return Observable
                .StartAsync(
                    async () =>
                    {
                        HttpResponseMessage response = await _client.PostAsync(uri, content);

                        response.EnsureSuccessStatusCode();

                        return await response.Content.ReadAsStreamAsync();
                    }
                )
                .SubscribeOn(_requestScheduler)
                .ObserveOn(_responseScheduler)
                .ToTask();
        }
    }
}
