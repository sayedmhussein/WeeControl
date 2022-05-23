using WeeControl.User.UserServiceCore.Enums;

namespace WeeControl.User.UserServiceCore.Interfaces;

public interface IDevicePageNavigation
{
    Task NavigateToAsync(PagesEnum page);
    Task NavigateToAsync(PagesEnum page, bool forceLoad);
    Task NavigateToAsync(PagesEnum page, bool forceLoad = false, bool disableBackButton = false);
    
    Task GoBackAsync();
}