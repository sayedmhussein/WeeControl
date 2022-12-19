using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ApplicationService.UnitTest")]
namespace WeeControl.Frontend.AppService.Internals.Interfaces;

internal interface IServerOperation
{
    Task<HttpResponseMessage> 
        GetResponseMessage(HttpMethod method, Version version, string relativeUri, bool accurateLocation = false);
    
    Task<HttpResponseMessage> 
        GetResponseMessage(HttpMethod method, Version version, string[] routeAndEndpoints, bool accurateLocation = false);
    
    Task<HttpResponseMessage> 
        GetResponseMessage<T>(HttpMethod method, Version version, string relativeUri, T dto, bool accurateLocation = false) where T : class;
    
    Task<HttpResponseMessage> 
        GetResponseMessage<T>(HttpMethod method, Version version, string[] routeAndEndpoints, T dto, bool accurateLocation = false) where T : class;
    
    Task<T?> ReadFromContent<T>(HttpContent content) where T : class;
    
    Task<bool> RefreshToken();
}