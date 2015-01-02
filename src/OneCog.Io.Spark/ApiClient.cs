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
            throw new NotImplementedException();
        }

        public Task<Stream> Post(Uri uri, HttpContent content)
        {
            throw new NotImplementedException();
        }
    }
}
