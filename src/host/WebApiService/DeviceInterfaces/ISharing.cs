namespace WeeControl.Host.WebApiService.DeviceInterfaces;

public interface ISharing
{
    Task CopyToClipboard(string text);
    Task<string> ReadFromClipboard();
    Task ClearClipboard();
}