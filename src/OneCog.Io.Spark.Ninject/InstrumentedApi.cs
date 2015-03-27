using EventSourceProxy;

namespace OneCog.Io.Spark.Ninject
{
    [EventSourceImplementation(Name="OneCog-Io-Spark-Ninject-Api")]
    public interface IInstrumentedApi : IApi { }

    public class InstrumentedApi : Api, IInstrumentedApi 
    {
        public InstrumentedApi(IRestClient restClient) : base(restClient) { }
    }
}
