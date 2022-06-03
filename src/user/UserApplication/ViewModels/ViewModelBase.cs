using System.ComponentModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.DataTransferObjects.Authentication;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.UserApplication.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public bool IsLoading { get; protected set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    
    private readonly IDevice device;
    
    protected ViewModelBase(IDevice device)
    {
        this.device = device;
    }
    
    protected async Task<HttpResponseMessage> SendMessageAsync<T>(HttpRequestMessage message, T? payload = null, bool accurateLocation = false) where T : class
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
    
    protected async Task<bool> RefreshTokenAsync()
    {
        if (await device.Security.IsAuthenticatedAsync() == false)
        {
            return false;
        }

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Authorization.Root)),
            Version = new Version("1.0"),
            Method = HttpMethod.Put
        };

        var response = await SendMessageAsync<object>(message);
        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDtoV1>>();
            var token = responseDto?.Payload?.Token;
            if (responseDto is not null && token is not null)
            {
                await device.Storage.SaveAsync(nameof(TokenDtoV1.Token), token);
                await device.Storage.SaveAsync(nameof(TokenDtoV1.FullName), responseDto?.Payload?.FullName ?? string.Empty);
                await device.Storage.SaveAsync(nameof(TokenDtoV1.PhotoUrl), responseDto?.Payload?.PhotoUrl ?? String.Empty);
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
    
    private  void UpdateHttpAuthorizationHeader(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return;
        
        device.Server.HttpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Brear", token);
    }
}