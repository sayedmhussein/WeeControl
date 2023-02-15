using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Internals.Services;

internal class ServerService : IServerOperation
{
    private readonly ICommunication communication;
    private readonly IDeviceSecurity security;
    private readonly HttpClient httpClient;

    public ServerService(ICommunication communication, IDeviceSecurity security)
    {
        httpClient = communication.HttpClient;
        this.communication = communication;
        this.security = security;
    }
    
    public Task<HttpResponseMessage> GetResponseMessage(HttpMethod method, Version version, string route, string? endpoint = null,
        string[]? query = null)
    {
        throw new NotImplementedException();
    }

    public Task<HttpResponseMessage> GetResponseMessage<T>(HttpMethod method, Version version, T dto, string route, string? endpoint = null,
        string[]? query = null) where T : class
    {
        throw new NotImplementedException();
    }

    public async Task<T?> ReadFromContent<T>(HttpContent content) where T : class
    {
        try
        {
            var response = await content.ReadFromJsonAsync<ResponseDto<T>>();
            return response?.Payload;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return null;
    }

    public Task<bool> RefreshToken()
    {
        throw new NotImplementedException();
    }
    
    private async Task UpdateHttpAuthorizationHeader()
    {
        var token = await security.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token))
            return;

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Brear", token);
    }
    
    private string GetFullAddress(string relative, string? endPoint, string[]? query)
    {
        if (string.IsNullOrEmpty(communication.ServerUrl))
            throw new NullReferenceException("Server URL was not defined!");
        
        if (string.IsNullOrEmpty(relative))
            throw new NullReferenceException("Server Relative URI was not defined!");

        var address = new StringBuilder();
        address.Append(communication.ServerUrl);

        if (communication.ServerUrl[-1] != '/')
            address.Append('/');
        
        address.Append(relative);
        if (relative[-1] != '/')
            address.Append('/');

        if (!string.IsNullOrEmpty(endPoint))
        {
            address.Append(endPoint);
            if (endPoint[-1] != '/')
                address.Append('/');
        }

        if (query is not null && query.Length >= 1)
        {
            address.Append(string.Join("?=", query));
        }

        return address.ToString();
    }
}