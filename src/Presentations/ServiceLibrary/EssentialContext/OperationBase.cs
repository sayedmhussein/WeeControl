using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using WeeControl.Presentations.ServiceLibrary.Enums;
using WeeControl.Presentations.ServiceLibrary.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Presentations.ServiceLibrary.EssentialContext;

public abstract class OperationBase
{
    private readonly IDevice device;

    protected OperationBase(IDevice device)
    {
        this.device = device;
    }
    
    protected HttpContent ConvertObjectToJsonContent<T>(T? payload = null, string mediaType = "application/json") where T : class
    {
        var location = device.DeviceLocation.GetLastKnownLocationAsync();
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
        device.DeviceServerCommunication.HttpClient.DefaultRequestHeaders.Clear();
        device.DeviceServerCommunication.HttpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Brear", await device.DeviceStorage.GetAsync(UserDataEnum.Token));
    }

    protected async Task<HttpResponseMessage> SendMessageAsync(HttpRequestMessage message)
    {
        try
        {
            return await device.DeviceServerCommunication.HttpClient.SendAsync(message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}