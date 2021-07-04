using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.Applications.BaseLib.Interfaces
{
    /// <summary>
    /// To be implemented on each device, then registered as singleton.
    /// </summary>
    public interface IDevice : IDeviceCommunication, IDeviceStorage, IDeviceState, IDeviceAction
    {
    }
}
