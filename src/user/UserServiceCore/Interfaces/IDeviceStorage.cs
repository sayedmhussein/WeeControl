using WeeControl.User.UserServiceCore.Enums;

namespace WeeControl.User.UserServiceCore.Interfaces;

public interface IDeviceStorage
{
    Task SaveAsync(UserDataEnum name, string value);

    Task<string> GetAsync(UserDataEnum name);
        
    Task ClearAsync();
}