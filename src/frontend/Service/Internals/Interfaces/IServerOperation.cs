namespace WeeControl.Frontend.AppService.Internals.Interfaces;

internal interface IServerOperation
{
    string GetFullAddress(string relative);
    
    Task<HttpResponseMessage> Send(
        HttpRequestMessage message,
        bool accurateLocation = false);
    
    Task<HttpResponseMessage> Send<T>(
        HttpRequestMessage message,
        T payload,
        bool accurateLocation = false) where T : class;

    Task<T?> ReadFromContent<T>(HttpContent content) where T : class;
    
    Task<bool> IsTokenValid();
}