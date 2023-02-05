namespace WeeControl.Frontend.AppService.DeviceInterfaces;

public interface IStorage
{
    string CashDirectory { get; }
    string AppDataDirectory { get; }

    Task SaveKeyValue(string key, string value);

    Task<string> GetKeyValue(string key);

    Task ClearKeysValues();
}