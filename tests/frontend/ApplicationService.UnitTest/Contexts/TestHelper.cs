using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.Frontend.ApplicationService.Services;

namespace WeeControl.Frontend.ApplicationService.UnitTest.Contexts;

public sealed class TestHelper : IDisposable
{
    public DeviceServiceMock DeviceMock;
    public Mock<IServerOperation> ServerOperationMock;

    public TestHelper(string deviceName)
    {
        DeviceMock = new DeviceServiceMock(deviceName);
        ServerOperationMock = new Mock<IServerOperation>();
        
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