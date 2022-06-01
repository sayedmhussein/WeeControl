using System.Text;
using Newtonsoft.Json;

namespace WeeControl.User.UserApplication.Test.ViewModels;

public abstract class ViewModelTestsBase : IDisposable
{
    protected DeviceServiceMock mock;

    protected ViewModelTestsBase(string deviceName)
    {
        mock = new DeviceServiceMock(deviceName);
    }

    public void Dispose()
    {
        mock = null!;
    }
    
    protected HttpContent GetJsonContent<T>(T dto)
    {
        return new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
    }
}