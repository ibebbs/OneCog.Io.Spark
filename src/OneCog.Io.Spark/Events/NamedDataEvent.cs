using System;

namespace OneCog.Io.Spark.Events
{
    internal class NamedDataEvent : INamedDataEvent
    {
        public NamedDataEvent(string name, string deviceId, uint timeToLive, DateTime published, string data)
        {
            Name = name;
            DeviceId = deviceId;
            TimeToLive = timeToLive;
            Published = published;
            Data = data;
        }

        public string Name { get; private set; }

        public string DeviceId { get; private set; }

        public uint TimeToLive { get; private set; }

        public DateTime Published { get; private set; }

        public string Data { get; private set; }
    }
}
