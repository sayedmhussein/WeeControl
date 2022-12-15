namespace WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

public interface ISharing
{
    Task CopyToClipboard(string text);
    Task<string> ReadFromClipboard();
    Task ClearClipboard();
}