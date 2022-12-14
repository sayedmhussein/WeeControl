using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using WeeControl.Frontend.AppService.Interfaces;
using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;
using WeeControl.Frontend.AppService.Services;

namespace WeeControl.Frontend.Service.UnitTest.Contexts;

public sealed class TestHelper : IDisposable
{
    public DeviceServiceMock DeviceMock;

    public TestHelper(string deviceName)
    {
        DeviceMock = new DeviceServiceMock(deviceName);
    }

    public void Dispose()
    {
        DeviceMock = null!;
    }
    
    public HttpContent GetJsonContent<T>(T dto)
    {
        return new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
    }

    public IServerOperation GetServer(IDevice device)
    {
        return new ServerOperationService(device);
    }
}