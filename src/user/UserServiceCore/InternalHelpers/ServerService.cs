using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.InternalHelpers;

internal class ServerService : IServerService
{
    private readonly IDevice device;

    public ServerService(IDevice device)
    {
        this.device = device;
    }

    public Task<HttpResponseMessage> SendMessageAsync(HttpRequestMessage message, bool accurateLocation)
    {
        return SendMessageAsync<IRequestDto>(message, null, accurateLocation);
    }

    public async Task<HttpResponseMessage> SendMessageAsync<T>(HttpRequestMessage message, T? payload, bool accurateLocation) where T : class
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
            await device.Alert.DisplayAlert(AlertEnum.FailedToCommunicateWithServer);
            await device.Navigation.NavigateToAsync(PagesEnum.NoInternet);
            return new HttpResponseMessage(HttpStatusCode.BadGateway);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<T?> GetObjectFromJsonResponseAsync<T>(HttpResponseMessage message)
    {
        return message.Content.ReadFromJsonAsync<T>();
    }
    
    private HttpContent ConvertObjectToJsonContent<T>(string mediaType, double? latitude, double? longitude, T? payload = null) where T : class
    {
        var location = device.Location.GetLastKnownLocationAsync();
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