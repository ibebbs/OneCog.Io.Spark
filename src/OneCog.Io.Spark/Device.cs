using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Spark
{
    public interface IDevice
    {
        Task<Fallible<T>> ReadVariable<T>(string variableName);

        Task<Fallible<long>> Call(string functionName, string arguments);

        IObservable<string> Events(string named);

        IObservable<string> Events();
    }

    internal class Device : IDevice
    {
        private IApi _api;
        private readonly string _deviceId;

        public Device(IApi api, string deviceId)
        {
            _api = api;
            _deviceId = deviceId;
        }

        public async Task<Fallible<T>> ReadVariable<T>(string variableName)
        {
            Fallible<IVariable> variable = await _api.ReadVariable(_deviceId, variableName);

            return Fallible.Map(variable, value => value.As<T>());
        }

        public async Task<Fallible<long>> Call(string functionName, string arguments)
        {
            Fallible<IFunctionResult> result = await _api.CallFunction(_deviceId, functionName, arguments);

            return Fallible.Map(result, value => value.ReturnValue);
        }

        public IObservable<string> Events(string named)
        {
            throw new NotImplementedException();
        }

        public IObservable<string> Events()
        {
            throw new NotImplementedException();
        }
    }
}
