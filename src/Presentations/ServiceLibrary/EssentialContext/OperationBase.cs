using System.Net.Http.Headers;
using WeeControl.Presentations.ServiceLibrary.Enums;
using WeeControl.Presentations.ServiceLibrary.Interfaces;

namespace WeeControl.Presentations.ServiceLibrary.EssentialContext;

public abstract class OperationBase
{
    private readonly IEssentialDeviceServerDevice device;

    protected OperationBase(IEssentialDeviceServerDevice device)
    {
        this.device = device;
    }
    
    protected async Task UpdateAuthorizationAsync()
    {
        device.HttpClient.DefaultRequestHeaders.Clear();
        device.HttpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Brear", await device.GetAsync(UserDataEnum.Token));
    }

    protected async Task<HttpResponseMessage> SendMessageAsync(HttpRequestMessage message)
    {
        try
        {
            return await device.HttpClient.SendAsync(message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}