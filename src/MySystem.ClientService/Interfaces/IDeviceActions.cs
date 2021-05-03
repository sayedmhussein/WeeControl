using System;
using System.Threading.Tasks;

namespace MySystem.ClientService.Interfaces
{
    public interface IDeviceActions
    {
        void PlacePhoneCall(string number);

        Task DisplayMessageAsync(string title, string message);
        Task NavigateAsync(string pageName);

        object GetRequestDto<T>(T payload);
    }
}
