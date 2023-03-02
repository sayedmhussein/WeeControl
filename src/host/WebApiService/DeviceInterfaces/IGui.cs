namespace WeeControl.Host.WebApiService.DeviceInterfaces;

public interface IGui
{
    public string CurrentPageName { get; }
    
    Task DisplayAlert(string message);

    Task DisplayQuickAlert(string message);

    Task NavigateToAsync(string pageName, bool forceLoad = false);
}