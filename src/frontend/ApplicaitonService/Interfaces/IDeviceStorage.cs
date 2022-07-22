namespace WeeControl.User.UserApplication.Interfaces;

public interface IDeviceStorage
{
    Task SaveAsync(string key, string value);

    Task<string> GetAsync(string key);
        
    Task ClearAsync();
}