using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Internals.Services;

internal class ServerService : IServerOperation
{
    private readonly ICommunication communication;
    private readonly IDeviceSecurity security;
    private readonly IFeature feature;
    private readonly IGui gui;
    private readonly HttpClient httpClient;

    public ServerService(ICommunication communication, IDeviceSecurity security, IFeature feature, IGui gui)
    {
        httpClient = communication.HttpClient;
        this.communication = communication;
        this.security = security;
        this.feature = feature;
        this.gui = gui;
    }
    
    public Task<HttpResponseMessage> GetResponseMessage(HttpMethod method, Version version, string route, string? endpoint = null,
        string[]? query = null)
    {
        var address = GetFullAddress(route, endpoint, query);
        
        return Send(method, version, new Uri(address));
    }

    public async Task<HttpResponseMessage> GetResponseMessage<T>(HttpMethod method, Version version, T dto, string route, string? endpoint = null,
        string[]? query = null) where T : class
    {
        var address = GetFullAddress(route, endpoint, query);
        
        var location = await feature.GetDeviceLocation();
        var payload = RequestDto.Create(dto, await feature.GetDeviceId(), location.Latitude, location.Longitude);
        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        return await Send(method, version, new Uri(address), content);
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

    public async Task<bool> RefreshToken()
    {
        if (await security.IsAuthenticated() == false)
            return false;

        var response = await GetResponseMessage(HttpMethod.Put, new Version("1.0"), ControllerApi.Essentials.Authorization.Route);
        if (response.IsSuccessStatusCode)
        {
            var dto = await ReadFromContent<TokenResponseDto>(response.Content);
            if (dto is not null)
            {
                var token = dto.Token;
                await security.UpdateToken(token);
                return true;
            }
        }

        if ((int) response.StatusCode < 500)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await security.DeleteToken();
                await gui.DisplayAlert("Please login again.");
                await gui.NavigateToAsync(ApplicationPages.Essential.LoginPage, forceLoad:true);
                return false;
            }
            
            await gui.DisplayAlert($"Unexpected error no. {response.StatusCode}");
        }

        return false;
    }

    private async Task<HttpResponseMessage> Send(HttpMethod method, Version version, Uri uri, HttpContent? content = null)
    {
        var requestMessage = new HttpRequestMessage()
        {
            Method = method, Version = version, RequestUri = uri, Content = content
        };

        await UpdateHttpAuthorizationHeader();

        var responseMessage = await communication.HttpClient.SendAsync(requestMessage);

        return responseMessage;
    }

    private async Task UpdateHttpAuthorizationHeader()
    {
        var token = await security.GetToken();
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

        if (communication.ServerUrl.Last() != '/')
            address.Append('/');
        
        address.Append(relative);
        
        if (!string.IsNullOrEmpty(endPoint))
        {
            if (relative.Last() != '/')
                address.Append('/');
            address.Append(endPoint);
        }

        if (query is not null && query.Length >= 1)
        {
            address.Append('?');
            for (int i = 0; i < query.Length; i++)
            {
                if (i % 2 == 0)
                {
                    address.Append(query[i]);
                    address.Append('=');
                }
                else
                {
                    address.Append($"{query[i]}");
                }
            }
        }
        
        

        return address.ToString();
    }
}