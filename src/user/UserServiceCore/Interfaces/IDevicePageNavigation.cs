using WeeControl.User.UserServiceCore.Enums;

namespace WeeControl.User.UserServiceCore.Interfaces;

public interface IDevicePageNavigation
{
    Task NavigateToAsync(string pageName, bool forceLoad = false);
    
    [Obsolete]
    Task NavigateToAsync(PagesEnum page);
    [Obsolete]
    Task NavigateToAsync(PagesEnum page, bool forceLoad);

    Task GoBackAsync();
}