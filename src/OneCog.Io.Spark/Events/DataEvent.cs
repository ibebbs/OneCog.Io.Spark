using System;

namespace OneCog.Io.Spark.Events
{
    public class DataEvent : IDataEvent
    {
        public DataEvent(string deviceId, uint timeToLive, DateTime published, string data)
        {
            DeviceId = deviceId;
            TimeToLive = timeToLive;
            Published = published;
            Data = data;
        }

        public string DeviceId { get; private set; }

        public uint TimeToLive { get; private set; }

        public DateTime Published { get; private set; }

        public string Data { get; private set; }
    }
}
