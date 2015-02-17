using EventSourceProxy;
using Ninject;
using Ninject.Modules;
using OneCog.Io.Spark;
using System.Reactive.Concurrency;

namespace OneCog.Io.Spark.Ninject
{
    public class Module : NinjectModule
    {
        private readonly string _accessToken;

        public Module(string accessToken)
        {
            _accessToken = accessToken;
        }

        public override void Load()
        {
            Bind<string>().ToConstant(_accessToken).WhenInjectedExactlyInto<InstrumentedRestClient>().InSingletonScope();
            Bind<IScheduler>().ToConstant(TaskPoolScheduler.Default).WhenInjectedExactlyInto<InstrumentedRestClient>().InSingletonScope();

            Bind<RestClient>().ToSelf();
            Bind<IRestClient>().ToMethod(context => TracingProxy.CreateWithActivityScope<IInstrumentedRestClient>(context.Kernel.Get<InstrumentedRestClient>())).InSingletonScope();

            Bind<Api>().ToSelf();
            Bind<IApi>().ToMethod(context => TracingProxy.CreateWithActivityScope<IInstrumentedApi>(context.Kernel.Get<InstrumentedApi>())).InSingletonScope();
        }
    }
}
