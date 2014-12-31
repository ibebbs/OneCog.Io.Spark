using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    [JsonObject("coreInfo")]
    public class CoreInfo
    {
        [JsonProperty("last_app")]
        public string LastApp { get; set; }

        [JsonProperty("last_heard")]
        public DateTime LastHeard { get; set; }

        [JsonProperty("connected")]
        public bool Connected { get; set; }

        [JsonProperty("deviceID")]
        public string DeviceId { get; set; }
    }
}
