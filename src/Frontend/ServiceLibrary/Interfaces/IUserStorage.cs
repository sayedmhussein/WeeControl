using WeeControl.Frontend.ServiceLibrary.Enums;

namespace WeeControl.Frontend.ServiceLibrary.Interfaces
{
    public interface IUserStorage
    {
        Task SaveAsync(UserDataEnum name, string value);

        Task<string> GetAsync(UserDataEnum name);
        
        Task ClearAsync();
    }
}