using WeeControl.Presentations.ServiceLibrary.Enums;

namespace WeeControl.Presentations.ServiceLibrary.Interfaces;

public interface IDeviceStorage
{
    Task SaveAsync(UserDataEnum name, string value);

    Task<string> GetAsync(UserDataEnum name);
        
    Task ClearAsync();
}