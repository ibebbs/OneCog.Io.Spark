using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    public interface ICloud
    {
        Task<IDevice> GetDevice(string deviceId);
    }

    public class Cloud
    {
        private readonly IApi _api;

        public Cloud(IApi api)
        {
            _api = api;
        }

        public Task<IDevice> GetDevice(string deviceId)
        {
            return Task.FromResult<IDevice>(new Device(_api, deviceId));
        }
    }
}
