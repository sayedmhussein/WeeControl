namespace WeeControl.Host.WebApiService.DeviceInterfaces;

public interface IMedia
{
    Task Speak(string message);
}