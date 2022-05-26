using System.ComponentModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.ViewModels;

public class ViewModelBase : INotifyPropertyChanged
{
    public bool IsLoading { get; protected set; } = false;
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly IDevice device;
    
    protected ViewModelBase(IDevice device)
    {
        this.device = device;
    }
    
    protected async Task<HttpResponseMessage> SendMessageAsync(HttpRequestMessage message, bool includeRequestDto = false, bool accurateLocation = false)
    {
        if (includeRequestDto)
        {
            var location = accurateLocation ? 
                await device.Location.GetAccurateLocationAsync() :
                await device.Location.GetLastKnownLocationAsync();
            
            message.Content = ConvertObjectToJsonContent<object>("application/json", location.Latitude, location.Longitude);
        }

        return await SendMessageAsync<IRequestDto>(message, null, accurateLocation);
    }

    protected async Task<HttpResponseMessage> SendMessageAsync<T>(HttpRequestMessage message, T? payload, bool accurateLocation = false) where T : class
    {
        if (message.Content is null && payload is not null)
        {
            var location = accurateLocation ? 
                await device.Location.GetAccurateLocationAsync() :
                await device.Location.GetLastKnownLocationAsync();
            
            message.Content = ConvertObjectToJsonContent("application/json", location.Latitude, location.Longitude, payload);
        }

        try
        {
            await UpdateAuthorizationAsync();
            return await device.Server.HttpClient.SendAsync(message);
        }
        catch (HttpRequestException e)
        {
            await device.Navigation.NavigateToAsync(Pages.Home.NoInternet);
            return new HttpResponseMessage(HttpStatusCode.BadGateway);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    protected Task<T?> GetObjectFromJsonResponseAsync<T>(HttpResponseMessage message)
    {
        return message.Content.ReadFromJsonAsync<T>();
    }

    protected async Task<bool> RefreshTokenAsync()
    {
        if (await device.Security.IsAuthenticatedAsync() == false)
        {
            return false;
        }

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Session)),
            Version = new Version("1.0"),
            Method = HttpMethod.Put
        };

        var response = await SendMessageAsync(message, includeRequestDto: true);
        if (response.IsSuccessStatusCode)
        {
            var responseDto = await GetObjectFromJsonResponseAsync<ResponseDto<TokenDtoV1>>(response);
            var token = responseDto?.Payload?.Token;
            if (responseDto is not null && token is not null)
            {
                await device.Storage.SaveAsync(UserDataEnum.Token, token);
                await device.Storage.SaveAsync(UserDataEnum.FullName, responseDto?.Payload?.FullName);
                await device.Storage.SaveAsync(UserDataEnum.PhotoUrl, responseDto?.Payload?.PhotoUrl);
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
    
    private HttpContent ConvertObjectToJsonContent<T>(string mediaType, double? latitude, double? longitude, T? payload = null) where T : class
    {
        var dto = payload == null ? 
            new RequestDto(device.DeviceId) : 
            new RequestDto<T>(device.DeviceId, payload, latitude, longitude);
        
        return new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, mediaType);
    }
    
    private async Task UpdateAuthorizationAsync()
    {
        device.Server.HttpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Brear", await device.Storage.GetAsync(UserDataEnum.Token));
    }
    
    
}