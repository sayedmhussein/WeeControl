using System;
using System.Threading.Tasks;

namespace WeeControl.Applications.BaseLib.Interfaces
{
    public interface IDeviceAction
    {
        Task PlacePhoneCallAsync(string number);
        Task NavigateToLocationAsync(double latitude, double longitude);
        Task OpenWebPageAsync(string url);

        //Messages to user
        enum Message { NoInternet, Logout, InternalError, ServerError };
        Task DisplayMessageAsync(Message message);
        Task DisplayMessageAsync(string title, string message);
        Task DisplayMessageAsync(string title, string message, string acceptButton);

        //Navigation
        Task NavigateToPageAsync(string pageName);
        Task TerminateAppAsync();
    }
}
