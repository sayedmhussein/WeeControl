namespace WeeControl.Host.WebApiService.DeviceInterfaces;

public interface IGui
{
    string CurrentPageName();
    
    Task DisplayAlert(string message);

    Task NavigateToAsync(string pageName, bool forceLoad = false);
}