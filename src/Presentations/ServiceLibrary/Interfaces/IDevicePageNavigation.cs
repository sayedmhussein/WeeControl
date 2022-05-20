using WeeControl.Presentations.ServiceLibrary.Enums;

namespace WeeControl.Presentations.ServiceLibrary.Interfaces;

public interface IDevicePageNavigation
{
    Task NavigateToAsync(PagesEnum page, bool force = false);
}