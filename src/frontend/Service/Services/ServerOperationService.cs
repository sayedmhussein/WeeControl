using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authentication;
using WeeControl.Common.SharedKernel.RequestsResponses;
using WeeControl.Frontend.AppService.Interfaces;
using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces.Obsolute;

[assembly: InternalsVisibleTo("ApplicationService.UnitTest")]
namespace WeeControl.Frontend.AppService.Services;

internal class ServerOperationService : IServerOperation
{
    private static HttpClient httpClient = new HttpClient();
    private readonly IGuiInterface guiInterface;
    private readonly IDeviceSecurity device;

    public ServerOperationService(IGuiInterface guiInterface, IDeviceSecurity device)
    {
        this.guiInterface = guiInterface;
        this.device = device;
    }

    public string GetFullAddress(string relative)
    {
        return guiInterface.ServerUrl + relative;
    }

    public Task<HttpResponseMessage> Send(HttpRequestMessage message, bool accurateLocation = false)
    {
        //todo: to confirm that message verb allow no content.
        return Send(message, new object(), accurateLocation);
    }

    public async Task<HttpResponseMessage> Send<T>(HttpRequestMessage message, T payload, bool accurateLocation = false) where T : class
    {
        if (message.Content is null && message.Method != HttpMethod.Get)
        {
            message.Content = await GetResponseDtoAsHttpContentAsync(accurateLocation, payload);
        }

        try
        {
            var bla = device;
            var dra = bla.GetTokenAsync();
            var token = await dra;
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

    public async Task<bool> IsTokenValid()
    {
        if (await device.IsAuthenticatedAsync() == false)
        {
            return false;
        }

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(GetFullAddress(Api.Essential.Authorization.Route)),
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
                await guiInterface.SaveAsync(nameof(TokenResponseDto.Token), token);
                await guiInterface.SaveAsync(nameof(TokenResponseDto.FullName),
                    responseDto?.Payload?.FullName ?? string.Empty);
                await device.UpdateTokenAsync(token);
                return true;
            }
            else
            {
                throw new NullReferenceException("Response DTO from server is null");
            }
        }
        
        if (response.StatusCode != HttpStatusCode.BadGateway)
        {
            await device.DeleteTokenAsync();
            await guiInterface.ClearAsync();
        }
        
        return false;
    }
    
    private async Task<(double? Latitude, double? Longitude)> GetCurrentLocationAsync(bool accurate)
    {
        var loc = await guiInterface.GetDeviceLocation(accurate);
        return (loc.Latitude, loc.Longitude);
    }
    
    private async Task<HttpContent> GetResponseDtoAsHttpContentAsync<T>(bool locationAccuracy, T? payload = null) where T : class
    {
        var location = await GetCurrentLocationAsync(locationAccuracy);
        var dto = payload == null ? 
            RequestDto.Create(await guiInterface.GetDeviceId(), location.Latitude, location.Longitude) : 
            RequestDto.Create(payload, await guiInterface.GetDeviceId(), location.Latitude, location.Longitude);
        
        return new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
    }
    
    private void UpdateHttpAuthorizationHeader(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return;
        
        httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Brear", token);
    }
}