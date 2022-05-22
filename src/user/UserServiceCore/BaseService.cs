using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore;

[Obsolete("Use server interface instead")]
public abstract class BaseService
{
    private readonly IDevice device;

    protected BaseService(IDevice device)
    {
        this.device = device;
    }
    
    protected HttpContent ConvertObjectToJsonContent<T>(T? payload = null, string mediaType = "application/json") where T : class
    {
        var location = device.Location.GetLastKnownLocationAsync();
        var dto = payload == null ? 
            new RequestDto(device.DeviceId) : 
            new RequestDto<T>(device.DeviceId, payload);
        
        return new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, mediaType);
    }

    protected Task<T?> GetObjectAsync<T>(HttpResponseMessage message)
    {
        return message.Content.ReadFromJsonAsync<T>();
    }
    
    protected async Task UpdateAuthorizationAsync()
    {
        device.Server.HttpClient.DefaultRequestHeaders.Clear();
        device.Server.HttpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Brear", await device.Storage.GetAsync(UserDataEnum.Token));
    }

    protected async Task<HttpResponseMessage> SendMessageAsync(HttpRequestMessage message)
    {
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
}