namespace WeeControl.Frontend.AppService.DeviceInterfaces;

public interface ISharing
{
    Task CopyToClipboard(string text);
    Task<string> ReadFromClipboard();
    Task ClearClipboard();
}