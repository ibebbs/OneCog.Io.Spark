using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    public interface ICoreInfo
    {
        string DeviceId { get; }
        bool Connected { get; }
        DateTime LastHeard { get; }
        string LastApp { get; }
    }

    [JsonObject("coreInfo")]
    public class JsonCoreInfo : ICoreInfo
    {
        [JsonProperty("deviceID")]
        public string DeviceId { get; set; }

        [JsonProperty("connected")]
        public bool Connected { get; set; }

        [JsonProperty("last_app")]
        public string LastApp { get; set; }

        [JsonProperty("last_heard")]
        public DateTime LastHeard { get; set; }
    }
}
