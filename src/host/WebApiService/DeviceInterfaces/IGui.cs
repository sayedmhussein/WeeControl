namespace WeeControl.Host.WebApiService.DeviceInterfaces;

public interface IGui
{
    enum Severity
    {
        Normal, Success, Info, Warning, Error
    }
    
    string CurrentPageName { get; }

    Task DisplayAlert(string message, Severity severity = Severity.Normal);

    Task DisplayQuickAlert(string message, Severity severity = Severity.Normal);

    Task NavigateToAsync(string pageName, bool forceLoad = false);
}