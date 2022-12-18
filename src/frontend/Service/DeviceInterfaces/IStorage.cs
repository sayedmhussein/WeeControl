namespace WeeControl.Frontend.AppService.DeviceInterfaces;

public interface IStorage
{
    string CashDirectory { get; }
    string AppDataDirectory { get; }
    
    Task SaveAsync(string key, string value);

    Task<string> GetAsync(string key);
        
    Task ClearAsync();
}