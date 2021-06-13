using System;
using System.Threading.Tasks;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.User.Employee.Interfaces
{
    /// <summary>
    /// To be implemented on each device, then registered as singleton.
    /// </summary>
    public interface IDevice
    {
        IRequestMetadata Metadata { get; }

        bool Internet { get; }

        [Obsolete]
        string DeviceId { get; }

        string Token { get; set; }

        string FullUserName { get; set; }

        //
        Task PlacePhoneCallAsync(string number);
        Task NavigateToLocationAsync(double latitude, double longitude);
        Task OpenWebPageAsync(string url);

        //Messages to user
        enum Message { NoInternet, InternalError };
        Task DisplayMessageAsync(Message message);
        Task DisplayMessageAsync(string title, string message);
        Task DisplayMessageAsync(string title, string message, string acceptButton);

        //Navigation
        Task NavigateToPageAsync(string pageName);
        Task TerminateAppAsync();
    }
}
