using WeeControl.User.UserServiceCore.Enums;

namespace WeeControl.User.UserServiceCore.Interfaces;

public interface IDevicePageNavigation
{
    Task NavigateToAsync(PagesEnum page, bool forceLoad = false);
}