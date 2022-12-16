using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

namespace WeeControl.Frontend.MauiApp.Services;

[Obsolete("Use GuiService class", true)]
public class SharingService : ISharing
{
    public Task CopyToClipboard(string text)
    {
        return Clipboard.Default.SetTextAsync(text);
    }

    public Task<string> ReadFromClipboard()
    {
        return Clipboard.Default.GetTextAsync();
    }

    public Task ClearClipboard()
    {
        return CopyToClipboard(null);
    }
}