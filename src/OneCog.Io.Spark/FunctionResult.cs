using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OneCog.Io.Spark
{
    public interface IFunctionResult
    {
        string DeviceId { get; }
        string Name { get; }
        bool Connected { get; }
        long ReturnValue { get; }
    }

    internal class JsonFunctionResult : IFunctionResult
    {
        [JsonProperty("id")]
        public string DeviceId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("connected")]
        public bool Connected { get; set; }

        [JsonProperty("return_value")]
        public long ReturnValue { get; set; }
    }

    public static class Function
    {
        /*
           {
              "id": "0123456789abcdef01234567",
              "name": "prototype99",
              "connected": true,
              "return_value": 42
            } 
        */

        private static readonly JsonSerializer Serialiser = new JsonSerializer();

        public static IFunctionResult FromJsonStream(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                using (JsonTextReader text = new JsonTextReader(reader))
                {
                    return Serialiser.Deserialize<JsonFunctionResult>(text);
                }
            }
        }

        public static Uri Identifier(string deviceId, string name)
        {
            UriBuilder builder = new UriBuilder(Api.Protocol, Api.BaseAddress);
            builder.Path = string.Format("{0}/{1}/{2}/{3}", Api.VersionPath, Api.DevicesPath, deviceId, name);
            return builder.Uri;
        }

        public static string Arguments(string arguments)
        {
            string args = string.Format("{0}={1}", "args", arguments);

            return args;
        }
    }
}
