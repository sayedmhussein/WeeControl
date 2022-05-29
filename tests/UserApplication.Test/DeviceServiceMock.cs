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
    public string _token { get; set; }= string.Empty;

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
        TempMockForSecurity();
        
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
        var content1 = new StringContent(JsonConvert.SerializeObject(dataTransferObject), Encoding.UTF8, "application/json");
        var content2 = new StringContent(JsonConvert.SerializeObject(dataTransferObject), Encoding.UTF8, "application/json");

        var sequenceResponse = new List<Tuple<HttpStatusCode, HttpContent>>
        {
            new Tuple<HttpStatusCode, HttpContent>(statusCode, content1),
            new Tuple<HttpStatusCode, HttpContent>(statusCode, content2)
        };
        
        
        var client = GetHttpClientWithHttpMessageHandlerSequenceResponseMock(sequenceResponse);

        return GetObject(client);
    }
    
    private HttpClient GetHttpClientWithHttpMessageHandlerSequenceResponseMock(List<Tuple<HttpStatusCode,HttpContent>> returns)
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
        // use real http client with mocked handler here
        var httpClient = new HttpClient(handlerMock.Object)
        {
            //BaseAddress = new Uri("http://test.com/"),
            //BaseAddress = new Uri(GetLocalIpAddress())
        };
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

    private void TempMockForSecurity()
    {
        SecurityMock.Setup(x => x.UpdateTokenAsync(It.IsAny<string>()))
            .Callback((string tkn) =>
            {
                InjectTokenToMock(tkn);
            });
        SecurityMock.Setup(x => x.DeleteTokenAsync())
            .Callback(() =>
            {
                InjectTokenToMock(string.Empty);
            });
    }

    public void InjectTokenToMock(string tkn)
    {
        SecurityMock.SetupGet(x => x.Token).Returns(tkn);
        SecurityMock.Setup(x => x.IsAuthenticatedAsync()).ReturnsAsync(!string.IsNullOrEmpty(tkn));
        SecurityMock.Setup(x => x.GetTokenAsync()).ReturnsAsync(tkn);
        //StorageMock.Setup(x => x.GetAsync(nameof(TokenDtoV1.Token))).ReturnsAsync(tkn);
    }
}