using EventSourceProxy;
using System.Reactive.Concurrency;

namespace OneCog.Io.Spark.Ninject
{
    [EventSourceImplementation(Name="OneCog-Io-Spark-Ninject")]
    public interface IInstrumentedRestClient : IRestClient { }

    public class InstrumentedRestClient : RestClient, IInstrumentedRestClient 
    {
        public InstrumentedRestClient(string accessToken, IScheduler requestScheduler = null, IScheduler responseScheduler = null) : base(accessToken, requestScheduler, responseScheduler) { }
    }
}
