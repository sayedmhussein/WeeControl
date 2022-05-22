using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using Moq;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.ServiceCore.Test.Integration;

public class DeviceServiceMock
{
    public Mock<IDevice> DeviceMock { get; }
    public Mock<IDeviceAlert> AlertMock { get; }
    public Mock<IDeviceLocation> LocationMock { get; }
    public Mock<IDeviceSecurity> SecurityMock { get; }
    public Mock<IDeviceStorage> StorageMock { get; }
    public Mock<IDevicePageNavigation> NavigationMock { get; }
    public Mock<IDeviceServerCommunication> ServerMock { get; }
    
    public DeviceServiceMock(string device)
    {
        DeviceMock = new Mock<IDevice>();
        DeviceMock.SetupAllProperties();
        DeviceMock.Setup(x => x.CurrentTs).Returns(DateTime.Now);
        DeviceMock.Setup(x => x.DeviceId).Returns(device);
        
        AlertMock = new Mock<IDeviceAlert>();
        AlertMock.SetupAllProperties();

        LocationMock = new Mock<IDeviceLocation>();
        LocationMock.SetupAllProperties();

        SecurityMock = new Mock<IDeviceSecurity>();
        SecurityMock.SetupAllProperties();

        StorageMock = new Mock<IDeviceStorage>();
        StorageMock.SetupAllProperties();

        NavigationMock = new Mock<IDevicePageNavigation>();
        NavigationMock.SetupAllProperties();
        
        ServerMock = new Mock<IDeviceServerCommunication>();
        ServerMock.SetupAllProperties();
        ServerMock.Setup(x => x.ServerBaseAddress).Returns(GetLocalIpAddress());
        ServerMock.Setup(x => x.GetFullAddress(It.IsAny<string>())).Returns((string a) => GetLocalIpAddress() + a);
    }

    public IDevice GetObject(HttpClient httpClient)
    {
        ServerMock.Setup(x => x.HttpClient).Returns(httpClient);
        
        DeviceMock.Setup(x => x.Alert).Returns(AlertMock.Object);
        DeviceMock.Setup(x => x.Location).Returns(LocationMock.Object);
        DeviceMock.Setup(x => x.Security).Returns(SecurityMock.Object);
        DeviceMock.Setup(x => x.Storage).Returns(StorageMock.Object);
        DeviceMock.Setup(x => x.Navigation).Returns(NavigationMock.Object);
        DeviceMock.Setup(x => x.Server).Returns(ServerMock.Object);
        
        return DeviceMock.Object;
    }

    private static string GetLocalIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return "http://" + ip.ToString() + ":5000/";
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}