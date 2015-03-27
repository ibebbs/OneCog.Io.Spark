using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark.Events
{
    internal class StreamStartEventFactory : IFactory
    {
        public IEnumerable<IEvent> Create(string[] messages)
        {
            string header;

            if (messages.Length == 1 &&  Messages.TryParseHeader(messages[0], out header))
            {
                yield return new StreamStartEvent();
            }
        }
    }
}
