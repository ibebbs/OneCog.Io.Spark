using System;
using System.Collections.Generic;
using System.Linq;

namespace OneCog.Io.Spark.Events
{
    internal class NamedDataEventFactory : IFactory
    {
        public IEnumerable<IEvent> Create(string[] messages)
        {
            string name, data;

            if (messages.Length == 2 && Messages.TryParseEvent(messages[0], out name) && Messages.TryParseData(messages[1], out data))
            {
                EventData eventData = EventData.FromJson(data);

                yield return new NamedDataEvent(name, eventData.DeviceId, eventData.TimeToLive, eventData.Published, eventData.Data);
            }
        }
    }
}
