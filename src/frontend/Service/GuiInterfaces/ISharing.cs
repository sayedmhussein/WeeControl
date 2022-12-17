namespace WeeControl.Frontend.AppService.GuiInterfaces;

public interface ISharing
{
    Task CopyToClipboard(string text);
    Task<string> ReadFromClipboard();
    Task ClearClipboard();
}