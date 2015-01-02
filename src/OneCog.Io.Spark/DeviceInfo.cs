using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    public interface IDeviceInfo
    {
        string Id { get; }

        string Name { get; }

        bool Connected { get; }

        IEnumerable<object> Variables { get; }

        IEnumerable<object> Functions { get; }

        string WifiPatchVersion { get; }
    }

    public class JsonDeviceInfo : IDeviceInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("connected")]
        public bool Connected { get; set; }

        [JsonProperty("variables")]
        public IEnumerable<object> Variables { get; set; }

        [JsonProperty("functions")]
        public IEnumerable<object> Functions { get; set; }

        [JsonProperty("cc3000_patch_version")]
        public string WifiPatchVersion { get; set; }
    }

    public static class DeviceInfo
    {
        /*
         {
              "id": "53ff69066667574827472067",
              "name": "FirstCore",
              "connected": false,
              "variables": null,
              "functions": null,
              "cc3000_patch_version": "1.29"
            }
        */
        private static readonly JsonSerializer Serialiser = new JsonSerializer();

        public static Uri Identifier(string deviceId)
        {
            UriBuilder builder = new UriBuilder(Api.Protocol, Api.BaseAddress);
            builder.Path = string.Format("{0}/{1}/{2}", Api.VersionPath, Api.DevicesPath, deviceId);
            return builder.Uri;
        }

        public static IDeviceInfo FromJsonStream(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                using (JsonTextReader text = new JsonTextReader(reader))
                {
                    return Serialiser.Deserialize<JsonDeviceInfo>(text);
                }
            }
        }
    }
}
