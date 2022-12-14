namespace WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

public interface IDeviceStorage
{
    string LocalStorageLocation { get; }
    
    Task SaveAsync(string key, string value);

    Task<string> GetAsync(string key);
        
    Task ClearAsync();
}