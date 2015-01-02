using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    public interface IApiClient
    {
        Task<Stream> Get(Uri uri);

        Task<Stream> Post(Uri uri, HttpContent content);
    }

    public class ApiClient : IApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(string accessToken)
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
