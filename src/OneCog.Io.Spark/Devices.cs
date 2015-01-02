using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    public interface IDevicesInfo
    {
        string DeviceId { get; }
        string Name { get; }
        bool Connected { get; }
        DateTime LastHeard { get; }
        string LastApp { get; }
    }

    public class JsonDevicesInfo : IDevicesInfo
    {
        [JsonProperty("id")]
        public string DeviceId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("connected")]
        public bool Connected { get; set; }

        [JsonProperty("last_app")]
        public string LastApp { get; set; }

        [JsonProperty("last_heard")]
        public DateTime LastHeard { get; set; }
    }

    public static class Devices
    {
        private static readonly JsonSerializer Serialiser = new JsonSerializer();

        public static Uri Identifier()
        {
            UriBuilder builder = new UriBuilder(Api.Protocol, Api.BaseAddress);
            builder.Path = string.Format("{0}/{1}", Api.VersionPath, Api.DevicesPath);
            return builder.Uri;
        }

        public static IEnumerable<IDevicesInfo> FromJsonStream(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                using (JsonTextReader text = new JsonTextReader(reader))
                {
                    return Serialiser.Deserialize<List<JsonDevicesInfo>>(text);
                }
            }
        }
    }
}
