using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

namespace WeeControl.Frontend.MauiApp.Services;

[Obsolete("Use GuiService class", true)]
public class MediaService :IMedia
{
    public Task Speak(string message)
    {
        throw new NotImplementedException();
    }
}