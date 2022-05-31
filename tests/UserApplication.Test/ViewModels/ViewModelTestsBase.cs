using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using WeeControl.User.UserApplication.ViewModels;
using WeeControl.User.UserApplication.ViewModels.Authentication;

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