using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using Moq;
using WeeControl.Frontend.FunctionalService.Interfaces;

namespace WeeControl.Test.WebApi.Test.Functional;

public static class ApplicationMocks
{
    public static (
        Mock<IUserDevice>  userDevice,
        Mock<IUserCommunication> userCommunication,
        Mock<IUserStorage> userStorage
        ) GetMocks(HttpClient httpClient, string device)
    {
        return (GetUserDeviceMock(device), GetUserCommunicationMock(httpClient), GetUserStorageMockMock());
    }

    public static Mock<IUserDevice> GetUserDeviceMock(string device)
    {
        var mock = new Mock<IUserDevice>();
        mock.SetupAllProperties();
        mock.Setup(x => x.DeviceId).Returns(device);
        mock.Setup(x => x.TimeStamp).Returns(DateTime.UtcNow);
        return mock;
    }

    public static Mock<IUserCommunication> GetUserCommunicationMock(HttpClient httpClient)
    {
        var mock = new Mock<IUserCommunication>();
        mock.SetupAllProperties();
        mock.Setup(x => x.ServerBaseAddress).Returns(GetLocalIpAddress());
        mock.Setup(x => x.HttpClient).Returns(httpClient);
        return mock;
    }

    public static Mock<IUserStorage> GetUserStorageMockMock()
    {
        var mock = new Mock<IUserStorage>();
        mock.SetupAllProperties();
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