using WeeControl.Presentations.ServiceLibrary.Enums;

namespace WeeControl.Presentations.ServiceLibrary.Interfaces;

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