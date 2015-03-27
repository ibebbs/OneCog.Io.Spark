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
using System.Threading;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    public interface IApi
    {
        Task<IEnumerable<IDevicesInfo>> GetCores();
        Task<Fallible<IVariable>> ReadVariable(string deviceId, string variableName);
        IObservable<Fallible<IVariable>> ObserveVariable(string deviceId, string variableName, TimeSpan interval, IScheduler scheduler = null);
        Task<Fallible<IFunctionResult>> CallFunction(string deviceId, string functionName, string arguments);
    }

    public class Api : IApi
    {
        public static readonly string Protocol = "https://";
        public static readonly string BaseAddress = "api.spark.io";
        public static readonly string VersionPath = "v1";
        public static readonly string DevicesPath = "devices";
        public static readonly string EventsPath = "events";
        
        private readonly IRestClient _apiClient;
        private readonly IEventFactory _eventFactory;

        public Api(IRestClient apiClient, IEventFactory eventFactory)
        {
            _apiClient = apiClient;
            _eventFactory = eventFactory;
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

        public Task<Fallible<IVariable>> ReadVariable(string deviceId, string variableName)
        {
            Uri uri = Variable.Identifier(deviceId, variableName);

            return Fallible.FromOperationAsync(async () => 
                {                    
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
            );
        }

        public IObservable<Fallible<IVariable>> ObserveVariable(string deviceId, string variableName, TimeSpan interval, IScheduler scheduler = null)
        {
            scheduler = scheduler ?? TaskPoolScheduler.Default;

            IObservable<Fallible<IVariable>> observable = Observable.Interval(interval, scheduler).StartWith(0).SelectMany(_ => ReadVariable(deviceId, variableName));

            return observable;
        }

        public Task<Fallible<IFunctionResult>> CallFunction(string deviceId, string functionName, string arguments)
        {
            Uri uri = Function.Identifier(deviceId, functionName);
            string args = Function.Arguments(arguments);

            return Fallible.FromOperationAsync(
                async () => 
                {
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
            );
        }

        private Task<IObservable<Fallible<IEvent>>> ParseStream(Stream stream, CancellationToken ct)
        {
            IObservable<string> observable = Observable.Using(
                () => new StreamReader(stream),
                reader => Observable.Generate(reader, r => !r.EndOfStream, r => r, r => r.ReadLine())
            ).Publish().RefCount();

            IObservable<Fallible<IEvent>> events = observable.Window(observable.Where(string.IsNullOrWhiteSpace))
                .SelectMany(window => window.Where(line => !string.IsNullOrWhiteSpace(line)).ToArray())
                .SelectMany(_eventFactory.ConstructEvent)
                .Select(@event => Fallible.FromValue(@event));

            return Task.FromResult(events);
        }

        public IObservable<Fallible<IEvent>> ObserveEvents(string eventName, string deviceId)
        {
            Uri uri = Event.Identifier(eventName, deviceId);

            return Observable
                .Using(async ct => await _apiClient.Get(uri), ParseStream);
        }
    }
}
