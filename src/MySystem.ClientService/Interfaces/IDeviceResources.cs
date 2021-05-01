using System;
using System.Net.Http;

namespace MySystem.ClientService.Interfaces
{
    public interface IDeviceResources
    {
        HttpClient ApiClient { get; }
    }
}
