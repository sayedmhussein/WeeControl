namespace WeeControl.Frontend.ApplicationService.Interfaces;

public interface IServerOperation
{
    Task<HttpResponseMessage> Send<T>(
        HttpRequestMessage message,
        T? payload = null,
        bool accurateLocation = false) where T : class;
    
    Task<bool> IsTokenValid();
}