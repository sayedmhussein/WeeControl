using System;
using System.Net.Http;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.Applications.BaseLib.Interfaces
{
    public interface IDeviceCommunication
    {
        bool Internet { get; }

        HttpClient HttpClientInstance { get; }
    }
}
