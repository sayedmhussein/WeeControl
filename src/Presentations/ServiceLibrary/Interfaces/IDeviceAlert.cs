namespace WeeControl.Presentations.ServiceLibrary.Interfaces;

public interface IDeviceAlert
{
    Task DisplaySimpleAlertAsync(string message);
    Task<bool> DisplayBooleanAlertAsync(string message);
    Task<string> DisplayPromptedAlertAsync(string message);
}