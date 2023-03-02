namespace WeeControl.Host.WebApiService.Internals.Interfaces;

internal interface IServerOperation
{
    Task<HttpResponseMessage>
        GetResponseMessage(
            HttpMethod method, 
            Version version, 
            string route, string? endpoint = null, string[]? query = null,
            bool includeRequestDto = false);
    
    Task<HttpResponseMessage>
        GetResponseMessage<T>(
            HttpMethod method, 
            Version version, 
            T dto, 
            string route, 
            string? endpoint = null, 
            string[]? query = null) where T : class;
    
    Task<T?> ReadFromContent<T>(HttpContent content) where T : class;

    Task<bool> RefreshToken();
}