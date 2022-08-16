using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Contexts.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Frontend.ApplicationService.Services;

public class ServerOperationService : IServerOperation
{
    private readonly IDevice device;

    public ServerOperationService(IDevice device)
    {
        this.device = device;
    }
    
    public async Task<HttpResponseMessage> Send<T>(HttpRequestMessage message, T? payload = default(T?), bool accurateLocation = false) where T : class
    {
        if (message.Content is null && message.Method != HttpMethod.Get)
        {
            message.Content = await GetResponseDtoAsHttpContentAsync(accurateLocation, payload);
        }

        try
        { 
            UpdateHttpAuthorizationHeader(await device.Security.GetTokenAsync());
            return await device.Server.HttpClient.SendAsync(message);
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

    public async Task<bool> IsTokenValid()
    {
        if (await device.Security.IsAuthenticatedAsync() == false)
        {
            return false;
        }

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Routes.Authorization)),
            Version = new Version("1.0"),
            Method = HttpMethod.Put
        };

        var response = await Send<object>(message);
        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<AuthenticationResponseDto>>();
            var token = responseDto?.Payload?.Token;
            if (responseDto is not null && token is not null)
            {
                await device.Storage.SaveAsync(nameof(AuthenticationResponseDto.Token), token);
                await device.Storage.SaveAsync(nameof(AuthenticationResponseDto.FullName),
                    responseDto?.Payload?.FullName ?? string.Empty);
                await device.Security.UpdateTokenAsync(token);
                return true;
            }
            else
            {
                throw new NullReferenceException("Response DTO from server is null");
            }
        }
        
        if (response.StatusCode != HttpStatusCode.BadGateway)
        {
            await device.Security.DeleteTokenAsync();
            await device.Storage.ClearAsync();
        }
        
        return false;
    }
    
    private Task<(double? Latitude, double? Longitude)> GetCurrentLocationAsync(bool accurate)
    {
        return accurate ? device.Location.GetAccurateLocationAsync() : device.Location.GetLastKnownLocationAsync();
    }
    
    private async Task<HttpContent> GetResponseDtoAsHttpContentAsync<T>(bool locationAccuracy, T? payload = null) where T : class
    {
        var location = await GetCurrentLocationAsync(locationAccuracy);
        var dto = payload == null ? 
            RequestDto.Create(device.DeviceId, location.Latitude, location.Longitude) : 
            RequestDto.Create(payload, device.DeviceId, location.Latitude, location.Longitude);
        
        return new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
    }
    
    private void UpdateHttpAuthorizationHeader(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return;
        
        device.Server.HttpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Brear", token);
    }
}