namespace WeeControl.Frontend.AppService.DeviceInterfaces;

public interface IMedia
{
    Task Speak(string message);
}