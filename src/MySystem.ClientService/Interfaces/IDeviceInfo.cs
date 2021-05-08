using System;
using System.Net.Http;
using System.Threading.Tasks;
using MySystem.SharedDto.V1;

namespace MySystem.ClientService.Interfaces
{
    public interface IDeviceInfo
    {
        bool InternetIsAvailable { get; }

        string DeviceId { get; }

        HttpClient HttpClient { get; }

        Task UpdateTokenAsync(string token);
    }
}
