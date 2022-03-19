using System.Threading.Tasks;

namespace WeeControl.Common.UserSecurityLib.Interfaces
{
    public interface IClientDevice
    {
        Task SaveTokenAsync(string token);
        Task<string> GetTokenAsync();

        Task SaveUserNameTask(string userName);
        Task SaveUserPhotoUrlAsync(string url);
        Task ClearUserDataAsync();
    }
}