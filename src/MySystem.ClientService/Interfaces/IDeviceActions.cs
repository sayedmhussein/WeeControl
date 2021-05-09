using System;
using System.Threading.Tasks;

namespace MySystem.ClientService.Interfaces
{
    public interface IDeviceAction
    {
        void PlacePhoneCall(string number);
        Task NavigateToLocationAsync(double latitude, double longitude);

        Task OpenWebPageAsync(string url);

        enum Message { NoInternet };
        Task DisplayMessageAsync(Message message);
        Task DisplayMessageAsync(string title, string message);
        Task DisplayMessageAsync(string title, string message, string acceptButton);

        void TerminateApp();
        Task NavigateToPageAsync(string pageName);
    }
}
