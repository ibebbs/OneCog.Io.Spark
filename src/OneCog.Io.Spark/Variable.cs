using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    public interface IVariable
    {
        T As<T>();

        string Command { get; }

        string Name { get; }

        object Result { get; }

        ICoreInfo CoreInfo { get; }
    }

    internal class JsonVariable : IVariable
    {
        public T As<T>()
        {
            return (T)Convert.ChangeType(Result, typeof(T));
        }

        ICoreInfo IVariable.CoreInfo
        {
            get { return CoreInfo; }
        }

        [JsonProperty("cmd")]
        public string Command { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("result")]
        public object Result { get; set; }

        [JsonProperty("coreInfo")]
        public JsonCoreInfo CoreInfo { get; set; }
    }


    public static class Variable
    {
        /*
        {
          "cmd": "VarReturn",
          "name": "temperature",
          "result": 42,
          "coreInfo": {
            "last_app": "",
            "last_heard": "2014-08-22T22:33:25.407Z",
            "connected": true,
            "deviceID": "53ff6c065075535119511687"
        }
        */
        private static readonly JsonSerializer Serialiser = new JsonSerializer();

        public static IVariable FromJsonStream(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                using (JsonTextReader text = new JsonTextReader(reader))
                {
                    return Serialiser.Deserialize<JsonVariable>(text);
                }
            }
        }

        public static string ToJsonString(IVariable variable)
        {
            JsonVariable jsonVariable = new JsonVariable
            {
                Name = variable.Name,
                Command = variable.Command,
                Result = variable.Result,
                CoreInfo = new JsonCoreInfo
                {
                    DeviceId = variable.CoreInfo.DeviceId,
                    Connected = variable.CoreInfo.Connected,
                    LastHeard = variable.CoreInfo.LastHeard,
                    LastApp = variable.CoreInfo.LastApp
                }
            };

            using (StringWriter stringWriter = new StringWriter())
            {
                using (JsonTextWriter writer = new JsonTextWriter(stringWriter))
                {
                    writer.Formatting = Formatting.Indented;

                    Serialiser.Serialize(writer, jsonVariable);
                }

                return stringWriter.ToString();
            }
        }

        public static Uri Identifier(string deviceId, string name)
        {
            UriBuilder builder = new UriBuilder(Api.Protocol, Api.BaseAddress);
            builder.Path = string.Format("{0}/{1}/{2}/{3}", Api.VersionPath, Api.DevicesPath, deviceId, name);
            return builder.Uri;
        }
    }
}
