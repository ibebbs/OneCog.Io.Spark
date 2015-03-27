using Newtonsoft.Json;
using System;
using System.IO;

namespace OneCog.Io.Spark.Events
{
    public class EventData
    {
        private static readonly JsonSerializer Serializer = new JsonSerializer();

        public static EventData FromJson(string json)
        {
            using (StringReader stringReader = new StringReader(json))
            {
                using (JsonReader jsonReader = new JsonTextReader(stringReader))
                {
                    return Serializer.Deserialize<EventData>(jsonReader);
                }
            }
        }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("ttl")]
        public uint TimeToLive { get; set; }

        [JsonProperty("published_at")]
        public DateTime Published { get; set; }

        [JsonProperty("coreid")]
        public string DeviceId { get; set; }
    }
}
