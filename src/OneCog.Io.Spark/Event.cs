using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    internal static class Event
    {
        private static IEnumerable<string> Paths(string eventName, bool owned, string deviceId)
        {
            yield return Api.VersionPath;

            if (owned)
            {
                yield return Api.DevicesPath;
            }

            if (!string.IsNullOrWhiteSpace(deviceId))
            {
                yield return deviceId;
            }

            yield return Api.EventsPath;

            if (!string.IsNullOrWhiteSpace(eventName))
            {
                yield return eventName;
            }
        }

        public static Uri Identifier(string eventName, string deviceId)
        {
            UriBuilder builder = new UriBuilder(Api.Protocol, Api.BaseAddress);
            builder.Path = string.Join("/", Paths(eventName, string.IsNullOrWhiteSpace(deviceId), deviceId));
            return builder.Uri;
        }

        public static Uri Identifier(string eventName, bool owned)
        {
            UriBuilder builder = new UriBuilder(Api.Protocol, Api.BaseAddress);
            builder.Path = string.Join("/", Paths(eventName, owned, null));
            return builder.Uri;
        }
    }
}
