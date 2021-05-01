using System;
using System.Net.Http;
using System.Threading.Tasks;
using MySystem.SharedDto.V1;

namespace MySystem.ClientService.Interfaces
{
    public interface IDeviceInfoService
    {
        string Token { get; set; }

        bool InternetAvailable { get; }

        RequestDto<T> GetRequestDto<T>(T payload);

        

        IApiUri ApiUni { get; }
    }
}
