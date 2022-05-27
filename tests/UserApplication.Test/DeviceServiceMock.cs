using System.Net;
using System.Net.Sockets;
using System.Text;
using Moq.Protected;
using Newtonsoft.Json;
using WeeControl.SharedKernel.DataTransferObjects.Authentication;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.UserApplication.Test;

public class DeviceServiceMock
{
    private static string _token = string.Empty;
    public Mock<IDevice> DeviceMock { get; }
    public Mock<IDeviceAlert> AlertMock { get; }
    public Mock<IDeviceLocation> LocationMock { get; }
    public Mock<IDeviceSecurity> SecurityMock { get; }
    public Mock<IDeviceStorage> StorageMock { get; }
    public Mock<IDevicePageNavigation> NavigationMock { get; }
    public Mock<IDeviceServerCommunication> ServerMock { get; }
    
    public Mock<HttpMessageHandler> HttpMessageHandlerMock { get; }
    
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
        SecurityMock.Setup(x => x.UpdateTokenAsync(It.IsAny<string>()))
            .Callback((string tkn) => _token = tkn);
        SecurityMock.Setup(x => x.DeleteTokenAsync())
            .Callback(() => _token = string.Empty);
        SecurityMock.Setup(x => x.IsAuthenticatedAsync()).ReturnsAsync(!string.IsNullOrEmpty(_token));

        StorageMock = new Mock<IDeviceStorage>();
        StorageMock.SetupAllProperties();
        StorageMock.Setup(x => x.SaveAsync(nameof(TokenDtoV1.Token), It.IsAny<string>()))
            .Callback((string key, string tkn) => _token = tkn);
        StorageMock.Setup(x => x.GetAsync(nameof(TokenDtoV1.Token))).ReturnsAsync(_token);

        NavigationMock = new Mock<IDevicePageNavigation>();
        NavigationMock.SetupAllProperties();
        
        ServerMock = new Mock<IDeviceServerCommunication>();
        ServerMock.SetupAllProperties();
        ServerMock.Setup(x => x.GetFullAddress(It.IsAny<string>())).Returns((string a) => GetLocalIpAddress() + a);
        
        HttpMessageHandlerMock = new Mock<HttpMessageHandler>();
    }

    public void SetupHttpMessageHandler<T>(HttpStatusCode statusCode, T dataTransferObject)
    {
        var content = new StringContent(JsonConvert.SerializeObject(dataTransferObject), Encoding.UTF8, "application/json");
        
        var response = new HttpResponseMessage
        {
            StatusCode = statusCode, 
            Content = content
        };
        
        HttpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
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

    public IDevice GetObject<T>(HttpStatusCode statusCode, T dataTransferObject)
    {
        var content = new StringContent(JsonConvert.SerializeObject(dataTransferObject), Encoding.UTF8, "application/json");

        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = statusCode, 
            Content = content
        };
        
        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
        
        var client = new HttpClient(httpMessageHandlerMock.Object);

        return GetObject(client);
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