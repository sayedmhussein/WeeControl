using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Moq.Protected;
using WeeControl.Frontend.ApplicationService.Interfaces;

namespace WeeControl.Frontend.ApplicationService.UnitTest;

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
        LocationMock.Setup(x => x.GetAccurateLocationAsync()).ReturnsAsync((0, 0));
        LocationMock.Setup(x => x.GetLastKnownLocationAsync()).ReturnsAsync((null,null));
        
        SecurityMock = new Mock<IDeviceSecurity>();
        SecurityMock.SetupAllProperties();
        SecurityMock.Setup(x => x.IsAuthenticatedAsync()).ReturnsAsync(false);
        SecurityMock.Setup(x => x.GetTokenAsync()).ReturnsAsync(string.Empty);
        SecurityMock.Setup(x => x.UpdateTokenAsync(It.IsAny<string>()))
            .Callback((string tkn) => InjectTokenToMock(tkn));
        SecurityMock.Setup(x => x.DeleteTokenAsync())
            .Callback(() =>InjectTokenToMock(string.Empty));
        
        StorageMock = new Mock<IDeviceStorage>();
        StorageMock.SetupAllProperties();
        StorageMock.Setup(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string k, string v) =>
            {
                StorageMock.Setup(x => x.GetAsync(k)).ReturnsAsync(v);
            });
        

        NavigationMock = new Mock<IDevicePageNavigation>();
        NavigationMock.SetupAllProperties();
        
        ServerMock = new Mock<IDeviceServerCommunication>();
        ServerMock.SetupAllProperties();
        ServerMock.Setup(x => x.GetFullAddress(It.IsAny<string>()))
            .Returns((string a) => GetLocalIpAddress() + a);
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
    
    public IDevice GetObject(HttpStatusCode statusCode, HttpContent? content)
    {
        var response = new HttpResponseMessage();
        response.StatusCode = statusCode;
        if (content is not null)
        {
            response.Content = content;
        }

        var handlerMock = new Mock<HttpMessageHandler>();
        
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        return GetObject(new HttpClient(handlerMock.Object));
    }

    public IDevice GetObject(IEnumerable<Tuple<HttpStatusCode, HttpContent>> sequenceResponse)
    {
        var client = GetHttpClientWithHttpMessageHandlerSequenceResponseMock(sequenceResponse);

        return GetObject(client);
    }

    public void InjectTokenToMock(string tkn)
    {
        SecurityMock.SetupSequence(x => x.IsAuthenticatedAsync()).ReturnsAsync(!string.IsNullOrEmpty(tkn));
        SecurityMock.SetupSequence(x => x.GetTokenAsync()).ReturnsAsync(tkn);
    }
    
    private HttpClient GetHttpClientWithHttpMessageHandlerSequenceResponseMock(IEnumerable<Tuple<HttpStatusCode,HttpContent>> returns)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var handlerPart = handlerMock
            .Protected()
            // Setup the PROTECTED method to mock
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );

        foreach (var item in returns)
        {
            handlerPart = AddReturnPart(handlerPart,item.Item1,item.Item2);
        }
        handlerMock.Verify();
        
        var httpClient = new HttpClient(handlerMock.Object);
        return httpClient;
    }

    private Moq.Language.ISetupSequentialResult<Task<HttpResponseMessage>> AddReturnPart(Moq.Language.ISetupSequentialResult<Task<HttpResponseMessage>> handlerPart,
        HttpStatusCode statusCode, HttpContent content)
    {
        return handlerPart

            // prepare the expected response of the mocked http call
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = statusCode, // HttpStatusCode.Unauthorized,
                Content = content //new StringContent("[{'id':1,'value':'1'}]"),
            });
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