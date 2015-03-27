using System;

namespace OneCog.Io.Spark
{
    public interface IEvent
    {
    }

    public interface IDataEvent : IEvent
    {
        string DeviceId { get; }

        uint TimeToLive { get; }

        DateTime Published { get; }

        string Data { get; }
    }

    public interface INamedDataEvent : IDataEvent
    {
        string Name { get; }
    }
}
