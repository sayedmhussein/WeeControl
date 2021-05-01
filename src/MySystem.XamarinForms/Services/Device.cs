using System;
using System.Net.Http;
using System.Threading.Tasks;
using MySystem.ClientService.Interfaces;
using MySystem.SharedDto.V1;

namespace MySystem.XamarinForms.Services
{
    public class Device : IDeviceInfoService
    {
        public Device()
        {
        }

        public string Token { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool InternetAvailable => throw new NotImplementedException();

        public HttpClient ApiClient => throw new NotImplementedException();

        public IApiUri ApiUni => throw new NotImplementedException();

        public Task DisplayMessageAsync(string subject, string message)
        {
            throw new NotImplementedException();
        }

        public RequestDto<T> GetRequestDto<T>(T payload)
        {
            throw new NotImplementedException();
        }
    }
}
