using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    public interface IApi
    {
        Task<IEnumerable<IDevicesInfo>> GetCores();
        Task<IVariable> ReadVariable(string deviceId, string variableName);
        Task<IFunctionResult> CallFunction(string deviceId, string functionName, string arguments);
    }

    public class Api : IApi
    {
        public static readonly string Protocol = "https://";
        public static readonly string BaseAddress = "api.spark.io";
        public static readonly string VersionPath = "v1";
        public static readonly string DevicesPath = "devices";
        
        private readonly IApiClient _apiClient;

        public Api(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IEnumerable<IDevicesInfo>> GetCores()
        {
            Uri uri = Devices.Identifier();

            Stream stream = await _apiClient.Get(uri);

            try
            {
                return Devices.FromJsonStream(stream);
            }
            finally
            {
                stream.Dispose();
                stream = null;
            }
        }

        public async Task<IVariable> ReadVariable(string deviceId, string variableName)
        {
            Uri uri = Variable.Identifier(deviceId, variableName);

            Stream stream = await _apiClient.Get(uri);

            try
            {
                return Variable.FromJsonStream(stream);
            }
            finally
            {
                stream.Dispose();
                stream = null;
            }
        }

        public IObservable<IVariable> ObserveVariable(string deviceId, string variableName, TimeSpan interval, IScheduler scheduler = null)
        {
            scheduler = scheduler ?? TaskPoolScheduler.Default;

            IObservable<IVariable> observable = Observable.Interval(interval, scheduler).StartWith(0).SelectMany(_ => ReadVariable(deviceId, variableName));

            return observable;
        }

        public async Task<IFunctionResult> CallFunction(string deviceId, string functionName, string arguments)
        {
            Uri uri = Function.Identifier(deviceId, functionName);
            string args = Function.Arguments(arguments);

            Stream stream = await _apiClient.Post(uri, new StringContent(args));

            try
            {
                return Function.FromJsonStream(stream);
            }
            finally
            {
                stream.Dispose();
                stream = null;
            }
        }
    }
}
