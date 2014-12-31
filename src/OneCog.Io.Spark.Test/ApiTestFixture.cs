using FakeItEasy;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark.Test
{
    [TestFixture]
    public class ApiTestFixture
    {
        private IApiClient _apiClient;
        private Api _api;
 
        [SetUp]
        public void Setup()
        {
            _apiClient = A.Fake<IApiClient>();
            _api = new Api(_apiClient);
        }

        [Test]
        public async Task CanReadAVariable()
        {
            A.CallTo(() => _apiClient.Get(new Uri("https://api.spark.io/v1/devices/53ff6c065075535119511687/temperature"))).Returns(Task.FromResult(Resources.JsonVariable.ToStream()));

            IVariable variable = await _api.ReadVariable("53ff6c065075535119511687", "temperature");

            A.CallTo(() => _apiClient.Get(new Uri("https://api.spark.io/v1/devices/53ff6c065075535119511687/temperature"))).MustHaveHappened();

            Assert.That(variable, Is.Not.Null);
        }

        [Test]
        public void CanObserveAVariable()
        {
            A.CallTo(() => _apiClient.Get(new Uri("https://api.spark.io/v1/devices/53ff6c065075535119511687/temperature"))).ReturnsLazily(call => Task.FromResult(Resources.JsonVariable.ToStream()));

            TestScheduler scheduler = new TestScheduler();

            List<IVariable> actual = new List<IVariable>();

            IObservable<IVariable> observable = _api.ObserveVariable("53ff6c065075535119511687", "temperature", TimeSpan.FromSeconds(10), scheduler);

            observable.Subscribe(actual.Add);

            scheduler.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);

            A.CallTo(() => _apiClient.Get(new Uri("https://api.spark.io/v1/devices/53ff6c065075535119511687/temperature"))).MustHaveHappened(Repeated.Exactly.Once);
            Assert.That(actual.Count, Is.EqualTo(1));

            scheduler.AdvanceBy(TimeSpan.FromSeconds(9).Ticks);

            A.CallTo(() => _apiClient.Get(new Uri("https://api.spark.io/v1/devices/53ff6c065075535119511687/temperature"))).MustHaveHappened(Repeated.Exactly.Twice);
            Assert.That(actual.Count, Is.EqualTo(2));
        }
    }
}
