using WeeControl.User.UserServiceCore.Enums;

namespace WeeControl.User.UserServiceCore.Interfaces;

public interface IDeviceAlert
{
    Task DisplayAlert(AlertEnum alertEnum);

    [Obsolete]
    Task DisplaySimpleAlertAsync(string message);
    [Obsolete]
    Task<bool> DisplayBooleanAlertAsync(string message);
    [Obsolete]
    Task<string> DisplayPromptedAlertAsync(string message);
}