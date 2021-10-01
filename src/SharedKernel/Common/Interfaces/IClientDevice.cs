using System.Threading.Tasks;

namespace WeeControl.SharedKernel.Common.Interfaces
{
    public interface IClientDevice
    {
        string DeviceId { get; }
        
        Task SaveTokenAsync(string token);
        Task<string> GetTokenAsync();

        Task SaveUserNameTask(string userName);
        Task SaveUserPhotoUrlAsync(string url);
        Task ClearUserDataAsync();
    }
}