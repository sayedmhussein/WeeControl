using System;
using System.Threading.Tasks;

namespace MySystem.ClientService.Interfaces
{
    public interface IDeviceActionService
    {
        Task DisplayMessageAsync(string subject, string message);
    }
}
