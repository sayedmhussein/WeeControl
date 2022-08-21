namespace WeeControl.Frontend.ApplicationService.Interfaces;

public interface IDevicePageNavigation
{
    Task NavigateToAsync(string pageName, bool forceLoad = false);

    Task GoBackAsync();
}