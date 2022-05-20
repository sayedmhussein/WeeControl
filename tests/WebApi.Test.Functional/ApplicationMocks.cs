using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using Moq;
using WeeControl.Presentations.ServiceLibrary.Interfaces;

namespace WeeControl.WebApi.Test.Functional;

public static class ApplicationMocks
{
    public static Mock<IEssentialDeviceServerDevice> GetEssentialMock(HttpClient httpClient, string device)
    {
        var mock = new Mock<IEssentialDeviceServerDevice>();
        mock.SetupAllProperties();
        mock.Setup(x => x.DeviceId).Returns(device);
        mock.Setup(x => x.TimeStamp).Returns(DateTime.UtcNow);
        
        mock.Setup(x => x.ServerBaseAddress).Returns(GetLocalIpAddress());
        mock.Setup(x => x.HttpClient).Returns(httpClient);
        mock.Setup(x => x.GetFullAddress(It.IsAny<string>())).Returns((string a) => GetLocalIpAddress() + a);

        return mock;
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