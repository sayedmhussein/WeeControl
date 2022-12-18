using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq.Protected;
using Newtonsoft.Json;
using WeeControl.Common.SharedKernel.RequestsResponses;
using WeeControl.Frontend.AppService;
using WeeControl.Frontend.AppService.DeviceInterfaces;

namespace WeeControl.Frontend.Service.UnitTest;

public class TestHelper : IDisposable
{
    public Mock<IDeviceData> DeviceMock { get; private set; }

    public TestHelper(string deviceName)
    {
        DeviceMock = new Mock<IDeviceData>();
        DeviceMock.SetupAllProperties();
        
        //ICommunication
        DeviceMock.Setup(x => x.ServerUrl).Returns(GetLocalIpAddress());
        DeviceMock.Setup(x => x.IsConnectedToInternet()).ReturnsAsync(true);
        
        //IFeature
        DeviceMock.Setup(x => x.IsEnergySavingMode()).ReturnsAsync(false);
        DeviceMock.Setup(x => x.GetDeviceId()).ReturnsAsync(deviceName);
        DeviceMock.Setup(x => x.GetDeviceLocation(It.IsAny<bool>())).ReturnsAsync((null, null, null));
        DeviceMock.Setup(x => x.IsMockedDeviceLocation()).ReturnsAsync(false);
        
        //IGui
        //IMedia
        //ISharing
        
        //IStorage
        DeviceMock.Setup(x => x.CashDirectory)
            .Returns(AppDomain.CurrentDomain.BaseDirectory + @"CashDirectory");
        DeviceMock.Setup(x => x.AppDataDirectory)
            .Returns(AppDomain.CurrentDomain.BaseDirectory + @"AppDirectory");
        DeviceMock.Setup(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string a, string b) => DeviceMock.Setup(x => x.GetAsync(a)).ReturnsAsync(b));
    }

    void IDisposable.Dispose()
    {
        DeviceMock = null;
    }
    
    public T GetService<T>() where T : class
    {
        var collection = new ServiceCollection();
        collection.AddApplicationServices();
        
        collection.AddSingleton<IDeviceData>(DeviceMock.Object);
        collection.AddSingleton<IStorage>(DeviceMock.Object);
        
        using var scope = collection.BuildServiceProvider().CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }

    public T GetService<T>(HttpClient httpClient) where T : class
    {
        DeviceMock.Setup(x => x.HttpClient).Returns(httpClient);
        return GetService<T>();
    }

    public T GetService<T>(HttpStatusCode statusCode, HttpContent? content) where T : class
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

        return GetService<T>(new HttpClient(handlerMock.Object));
    }

    public T GetService<T>(HttpStatusCode statusCode, object? dto) where T : class
    {
        var content = new StringContent(JsonConvert.SerializeObject(ResponseDto.Create(dto)), Encoding.UTF8, "application/json");
        return GetService<T>(statusCode, content);
    }
    
    public T GetService<T>(IEnumerable<Tuple<HttpStatusCode, HttpContent>> sequenceResponse) where T : class
    {
        var client = GetHttpClientWithHttpMessageHandlerSequenceResponseMock(sequenceResponse);

        return GetService<T>(client);
    }
    
    public T GetService<T>(IEnumerable<Tuple<HttpStatusCode, object?>> sequenceResponse) where T : class
    {
        var expected = new List<Tuple<HttpStatusCode, HttpContent>>();

        foreach (var item in sequenceResponse)
        {
            var content = new StringContent(JsonConvert.SerializeObject(ResponseDto.Create(item.Item2)), Encoding.UTF8, "application/json");
            var t = new Tuple<HttpStatusCode, HttpContent>(item.Item1, content);
            expected.Add(t);
        }
        
        var client = GetHttpClientWithHttpMessageHandlerSequenceResponseMock(expected);

        return GetService<T>(client);
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