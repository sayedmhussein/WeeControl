namespace WeeControl.User.UserServiceCore.Interfaces;

public interface IDeviceAlert
{
    Task DisplayAlert(string message);
    
    [Obsolete]
    Task<bool> DisplayBooleanAlertAsync(string message);
    [Obsolete]
    Task<string> DisplayPromptedAlertAsync(string message);
}