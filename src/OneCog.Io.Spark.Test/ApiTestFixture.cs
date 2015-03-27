using FakeItEasy;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark.Test
{
    [TestFixture]
    public class ApiTestFixture
    {
        private IRestClient _apiClient;
        private IEventFactory _eventFactory;

        private Api _api;
 
        [SetUp]
        public void Setup()
        {
            _apiClient = A.Fake<IRestClient>();

            IEventFactory eventFactory = new EventFactory(
                new Events.IFactory[] {
                    new Events.StreamStartEventFactory(),
                    new Events.DataEventFactory(),
                    new Events.NamedDataEventFactory()
                }
            );

            _api = new Api(_apiClient, eventFactory);
        }

        [Test]
        public async Task CanReadAVariable()
        {
            A.CallTo(() => _apiClient.Get(new Uri("https://api.spark.io/v1/devices/53ff6c065075535119511687/temperature"))).Returns(Task.FromResult(Resources.JsonVariable.ToStream()));

            Fallible<IVariable> variable = await _api.ReadVariable("53ff6c065075535119511687", "temperature");

            A.CallTo(() => _apiClient.Get(new Uri("https://api.spark.io/v1/devices/53ff6c065075535119511687/temperature"))).MustHaveHappened();

            Assert.That(variable, Is.Not.Null);
            Assert.That(variable.HasValue, Is.True);
            Assert.That(variable.Value, Is.Not.Null);
        }

        [Test]
        public void CanObserveAVariable()
        {
            A.CallTo(() => _apiClient.Get(new Uri("https://api.spark.io/v1/devices/53ff6c065075535119511687/temperature"))).ReturnsLazily(call => Task.FromResult(Resources.JsonVariable.ToStream()));

            TestScheduler scheduler = new TestScheduler();

            List<Fallible<IVariable>> actual = new List<Fallible<IVariable>>();

            IObservable<Fallible<IVariable>> observable = _api.ObserveVariable("53ff6c065075535119511687", "temperature", TimeSpan.FromSeconds(10), scheduler);

            observable.Subscribe(actual.Add);

            scheduler.AdvanceBy(TimeSpan.FromSeconds(1).Ticks);

            A.CallTo(() => _apiClient.Get(new Uri("https://api.spark.io/v1/devices/53ff6c065075535119511687/temperature"))).MustHaveHappened(Repeated.Exactly.Once);
            Assert.That(actual.Count, Is.EqualTo(1));

            scheduler.AdvanceBy(TimeSpan.FromSeconds(9).Ticks);

            A.CallTo(() => _apiClient.Get(new Uri("https://api.spark.io/v1/devices/53ff6c065075535119511687/temperature"))).MustHaveHappened(Repeated.Exactly.Twice);
            Assert.That(actual.Count, Is.EqualTo(2));
        }

        [Test]
        public void ShouldPerpetuateVariableObservableException()
        {
            A.CallTo(() => _apiClient.Get(new Uri("https://api.spark.io/v1/devices/53ff6c065075535119511687/temperature"))).ReturnsLazily(call => TaskEx.FromException<Stream>(new InvalidOperationException()));

            TestScheduler scheduler = new TestScheduler();

            List<Fallible<IVariable>> actual = new List<Fallible<IVariable>>();

            IObservable<Fallible<IVariable>> observable = _api.ObserveVariable("53ff6c065075535119511687", "temperature", TimeSpan.FromSeconds(10), scheduler);

            observable.Subscribe(actual.Add);

            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual[0].HasFailed, Is.True);
            Assert.That(actual[0].Exception, Is.InstanceOf<InvalidOperationException>());
        }

        private static Stream ConstructSimpleStream()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}\n\n", Resources.StreamHeader);
            builder.AppendFormat("{0}\n{1}\n\n{2}\n{3}\n\n", Resources.Event0Name, Resources.Event0Data, Resources.Event1Name, Resources.Event1Data);

            return new MemoryStream(Encoding.UTF8.GetBytes(builder.ToString()));
        }

        [Test]
        public void CanObserveEvents()
        {
            Stream stream = ConstructSimpleStream();

            A.CallTo(() => _apiClient.Get(new Uri("https://api.spark.io/v1/[device]/events/[event]"))).Returns(Task.FromResult(stream));

            IObservable<IEvent> events = _api
                .ObserveEvents("[event]", "[device]")
                .Where(fallible => fallible.HasValue)
                .Select(fallible => fallible.Value);

            List<IEvent> actual = new List<IEvent>(
                events.ToEnumerable()
            );

            List<IEvent> expected = new List<IEvent>(
                new IEvent[] {
                    new Events.StreamStartEvent(),
                    new Events.NamedDataEvent("pPhoto", "53ff72065075535122081687", 60, new DateTime(2015, 03, 24, 08, 10, 24, 573, DateTimeKind.Utc), "{\"Photocell\"}"),
                    new Events.NamedDataEvent("pMisc", "53ff72065075535122081687", 60, new DateTime(2015, 03, 24, 08, 10, 24, 573, DateTimeKind.Utc), "{\"Temperature\": 10.2, \"SCL\": 1, \"RSSI\": -63}")
                }
            );

            Assert.AreEqual(actual.Count, expected.Count);
        }
    }
}
