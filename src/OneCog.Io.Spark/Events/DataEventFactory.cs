using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark.Events
{
    internal class DataEventFactory : IFactory
    {
        public IEnumerable<IEvent> Create(string[] messages)
        {
            string data;

            if (messages.Length == 1 && Messages.TryParseData(messages[0], out data))
            {
                EventData eventData = EventData.FromJson(data);

                yield return new DataEvent(eventData.DeviceId, eventData.TimeToLive, eventData.Published, eventData.Data);
            }
        }
    }
}
