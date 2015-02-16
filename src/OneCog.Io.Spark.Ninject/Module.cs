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
            Bind<string>().ToConstant(_accessToken).WhenInjectedExactlyInto<RestClient>().InSingletonScope();
            Bind<IScheduler>().ToConstant(TaskPoolScheduler.Default).WhenInjectedExactlyInto<RestClient>().InSingletonScope();

            Bind<RestClient>().ToSelf();
            Bind<IRestClient>().ToMethod(context => TracingProxy.CreateWithActivityScope<IRestClient>(context.Kernel.Get<RestClient>())).InSingletonScope();

            Bind<Api>().ToSelf();
            Bind<IApi>().ToMethod(context => TracingProxy.CreateWithActivityScope<IApi>(context.Kernel.Get<Api>())).InSingletonScope();
        }
    }
}
