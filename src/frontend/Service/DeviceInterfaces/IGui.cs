namespace WeeControl.Frontend.AppService.DeviceInterfaces;

public interface IGui
{
    Task DisplayAlert(string message);
    
    Task NavigateToAsync(string pageName, bool forceLoad = false);
}