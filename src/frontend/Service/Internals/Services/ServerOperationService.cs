using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Frontend.AppService.Internals.Interfaces;
using WeeControl.Host.WebApiService.Data;

[assembly: InternalsVisibleTo("ApplicationService.UnitTest")]
namespace WeeControl.Frontend.AppService.Internals.Services;

internal class ServerOperationService : IServerOperation
{
    private readonly HttpClient httpClient;
    private readonly IDeviceData deviceData;
    private readonly IDeviceSecurity security;

    public ServerOperationService(IDeviceData deviceData, IDeviceSecurity security)
    {
        httpClient = deviceData.HttpClient;
        this.deviceData = deviceData;
        this.security = security;
    }

    private string GetFullAddress(string relative)
    {
        if (string.IsNullOrEmpty(deviceData.ServerUrl))
            throw new NullReferenceException("Server URL was not defined!");

        return deviceData.ServerUrl + relative;
    }

    private Task<HttpResponseMessage> Send(HttpRequestMessage message, bool accurateLocation = false)
    {
        //todo: to confirm that message verb allow no content.
        return Send(message, new object(), accurateLocation);
    }

    private async Task<HttpResponseMessage> Send<T>(HttpRequestMessage message, T payload, bool accurateLocation = false) where T : class
    {
        if (message.Content is not null)
        {
            throw new ArgumentException("You can't pass another payload in message with existing content, either remove the content or use the overloaded function.");
        }

        message.Content = await GetResponseDtoAsHttpContentAsync(accurateLocation, payload);

        try
        {
            var token = await security.GetTokenAsync();
            UpdateHttpAuthorizationHeader(token);
            return await httpClient.SendAsync(message);
        }
        catch (HttpRequestException)
        {
            return new HttpResponseMessage(HttpStatusCode.BadGateway);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [Obsolete]
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
        if (await security.IsAuthenticatedAsync() == false)
        {
            return false;
        }

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(GetFullAddress(ApiRouting.AuthorizationRoute)),
            Version = new Version("1.0"),
            Method = HttpMethod.Put
        };

        var response = await Send(message);
        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenResponseDto>>();
            var token = responseDto?.Payload?.Token;
            if (responseDto is not null && token is not null)
            {
                await deviceData.SaveKeyValue(nameof(TokenResponseDto.Token), token);
                await deviceData.SaveKeyValue(nameof(TokenResponseDto.FullName),
                    responseDto.Payload?.FullName ?? string.Empty);
                await security.UpdateTokenAsync(token);
                return true;
            }
            else
            {
                throw new NullReferenceException("Response DTO from server is null");
            }
        }

        if (response.StatusCode != HttpStatusCode.BadGateway)
        {
            await security.DeleteTokenAsync();
            await deviceData.ClearKeysValues();
        }

        return false;
    }

    private async Task<HttpContent> GetResponseDtoAsHttpContentAsync<T>(bool locationAccuracy, T payload) where T : class
    {
        var location = await deviceData.GetDeviceLocation(locationAccuracy);
        var dto = RequestDto.Create(payload, await deviceData.GetDeviceId(), location.Latitude, location.Longitude);

        return new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
    }

    private void UpdateHttpAuthorizationHeader(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return;

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Brear", token);
    }

    public Task<HttpResponseMessage> GetResponseMessage
        (HttpMethod method, Version version, string relativeUri, bool accurateLocation = false)
    {
        var message = GetHttpRequestMessage(method, version, relativeUri);

        return GetHttpResponseMessage(message);
    }

    public Task<HttpResponseMessage> GetResponseMessage
        (HttpMethod method, Version version, string[] routeAndEndpoints, bool accurateLocation = false)
    {
        var message = GetHttpRequestMessage(method, version, string.Join('/', routeAndEndpoints));

        return GetHttpResponseMessage(message);
    }

    public async Task<HttpResponseMessage> GetResponseMessage<T>
    (HttpMethod method, Version version, string relativeUri, T dto,
        bool accurateLocation = false) where T : class
    {
        var message = GetHttpRequestMessage(method, version, relativeUri);

        if (message.Content is not null)
        {
            throw new ArgumentException("You can't pass another payload in message with existing content, either remove the content or use the overloaded function.");
        }

        message.Content = await GetResponseDtoAsHttpContentAsync(accurateLocation, dto);

        return await GetHttpResponseMessage(message);
    }

    public Task<HttpResponseMessage> GetResponseMessage<T>
    (HttpMethod method, Version version, string[] routeAndEndpoints, T dto,
        bool accurateLocation = false) where T : class
    {
        return GetResponseMessage(method, version, string.Join('/', routeAndEndpoints), dto, accurateLocation);
    }

    private HttpRequestMessage GetHttpRequestMessage(HttpMethod method, Version version, string relativeUri)
    {
        if (string.IsNullOrWhiteSpace(relativeUri))
            throw new InvalidEnumArgumentException("You must provide a valid relative uri for the API.");

        return new HttpRequestMessage()
        {
            Method = method,
            Version = version,
            RequestUri = new Uri(GetFullAddress(relativeUri))
        };
    }

    private async Task<HttpResponseMessage> GetHttpResponseMessage(HttpRequestMessage httpRequestMessage)
    {
        try
        {
            var token = await security.GetTokenAsync();
            UpdateHttpAuthorizationHeader(token);

            var response = await httpClient.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
                return response;

            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return response;
                default:
                    return response;
            }
        }
        catch (HttpRequestException)
        {
            return new HttpResponseMessage(HttpStatusCode.BadGateway);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}