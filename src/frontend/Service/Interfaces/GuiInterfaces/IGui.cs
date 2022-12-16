namespace WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;

public interface IGui
{
    Task DisplayAlert(string message);
    
    Task NavigateToAsync(string pageName, bool forceLoad = false);
}