using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    public interface ICloud
    {
        Task<IEnumerable<IDevice>> GetDevices();

        Task<IDevice> GetDevice(string deviceId);
    }

    public class Cloud : ICloud
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

        public async Task<IEnumerable<IDevice>> GetDevices()
        {
            IEnumerable<IDevicesInfo> cores = await _api.GetCores();

            return cores.Select(core => new Device(_api, core.DeviceId)).ToArray();
        }
    }
}
