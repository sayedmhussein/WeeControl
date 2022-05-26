namespace WeeControl.User.UserServiceCore.Interfaces;

internal interface IServerService
{
    Task<HttpResponseMessage> SendMessageAsync(HttpRequestMessage message, bool accurateLocation = false);
    Task<HttpResponseMessage> SendMessageAsync<T>(HttpRequestMessage message, T? payload, bool accurateLocation = false) where T : class;
    Task<T?> GetObjectFromJsonResponseAsync<T>(HttpResponseMessage message);
}