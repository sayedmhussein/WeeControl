using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using WeeControl.Frontend.Service.Interfaces;
using WeeControl.Frontend.Service.Services;

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