namespace WeeControl.Presentations.FunctionalService.Interfaces;

public interface IDisplayAlert
{
    Task DisplaySimpleAlertAsync(string message);
    Task<bool> DisplayBooleanAlertAsync(string message);
    Task<string> DisplayPromptedAlertAsync(string message);
}