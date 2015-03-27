using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    public interface IEventFactory
    {
        IEnumerable<IEvent> ConstructEvent(string[] messages);
    }

    internal class EventFactory : IEventFactory
    {
        private IEnumerable<Events.IFactory> _factories;

        public EventFactory(IEnumerable<Events.IFactory> factories)
        {
            _factories = (factories ?? Enumerable.Empty<Events.IFactory>()).ToArray();
        }

        public IEnumerable<IEvent> ConstructEvent(string[] messages)
        {
            return _factories.SelectMany(factory => factory.Create(messages)).ToArray();
        }
    }
}
