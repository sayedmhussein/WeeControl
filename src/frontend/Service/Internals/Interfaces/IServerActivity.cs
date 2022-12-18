namespace WeeControl.Frontend.AppService.Internals.Interfaces;

internal interface IServerActivity
{
    Task<HttpResponseMessage> 
        GetResponseMessage(HttpMethod method, Version version, string relativeUri, bool accurateLocation = false);
    
    Task<HttpResponseMessage> 
        GetResponseMessage<T>(HttpMethod method, Version version, string relativeUri, T dto, bool accurateLocation = false);
}