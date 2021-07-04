using System;
using System.Threading.Tasks;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.Applications.BaseLib.Interfaces
{
    public interface IDeviceState
    {
        Task<IRequestMetadata> GetMetadataAsync(bool exactLocation = false);
    }
}
