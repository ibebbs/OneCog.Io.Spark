using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public RestClient(string accessToken)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public Task<Stream> Get(Uri uri)
        {
            return _client.GetStreamAsync(uri);
        }

        public async Task<Stream> Post(Uri uri, HttpContent content)
        {
            HttpResponseMessage response = await _client.PostAsync(uri, content);
            
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync();
        }
    }
}
