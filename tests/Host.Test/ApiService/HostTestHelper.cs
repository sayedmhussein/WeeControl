﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Moq.Language;
using Moq.Protected;
using Newtonsoft.Json;
using WeeControl.Core.DomainModel.Essentials.Dto;
using WeeControl.Core.SharedKernel.DtoParent;
using WeeControl.Core.Test;
using WeeControl.Host.WebApiService;
using WeeControl.Host.WebApiService.Contexts.Essentials;
using WeeControl.Host.WebApiService.DeviceInterfaces;

namespace WeeControl.Host.Test.ApiService;

public class HostTestHelper : IDisposable
{
    private HostTestHelper()
    {
        CommunicationMock = new Mock<ICommunication>();
        CommunicationMock.Setup(x => x.ServerUrl).Returns(GetLocalIpAddress());
        CommunicationMock.Setup(x => x.IsConnectedToInternet()).ReturnsAsync(true);

        FeatureMock = new Mock<IFeature>();
        FeatureMock.Setup(x => x.IsMockedDeviceLocation()).ReturnsAsync(false);
        FeatureMock.Setup(x => x.IsEnergySavingMode()).ReturnsAsync(false);
        FeatureMock.Setup(x => x.GetDeviceId()).ReturnsAsync("Device ID");
        FeatureMock.Setup(x => x.GetDeviceLocation(false)).ReturnsAsync((null, null, null));

        GuiMock = new Mock<IGui>();
        MediaMock = new Mock<IMedia>();

        SharingMock = new Mock<ISharing>();
        SharingMock.Setup(x => x.CopyToClipboard(It.IsAny<string>()))
            .Callback((string a) => SharingMock.Setup(x => x.ReadFromClipboard()).ReturnsAsync(a));
        SharingMock.Setup(x => x.ClearClipboard())
            .Callback(() => SharingMock.Setup(x => x.ReadFromClipboard()).ReturnsAsync(string.Empty));

        StorageMock = new Mock<IStorage>();
        StorageMock.Setup(x => x.CashDirectory)
            .Returns(AppDomain.CurrentDomain.BaseDirectory + @"CashDirectory");
        StorageMock.Setup(x => x.AppDataDirectory)
            .Returns(AppDomain.CurrentDomain.BaseDirectory + @"AppDirectory");
        StorageMock.Setup(x => x.SaveKeyValue(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string a, string b) => StorageMock.Setup(x => x.GetKeyValue(a)).ReturnsAsync(b));
    }

    public HostTestHelper(HttpClient httpClient) : this()
    {
        CommunicationMock.Setup(x => x.HttpClient).Returns(httpClient);
    }

    public HostTestHelper(HttpStatusCode statusCode, object? dto = null) : this()
    {
        var response = new HttpResponseMessage();
        response.StatusCode = statusCode;
        if (dto is not null)
            response.Content = new StringContent(JsonConvert.SerializeObject(ResponseDto.Create(dto)), Encoding.UTF8,
                "application/json");

        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        CommunicationMock.Setup(x => x.HttpClient).Returns(new HttpClient(handlerMock.Object));
    }

    public HostTestHelper(IEnumerable<(HttpStatusCode statusCode, object? dto)> responses) : this()
    {
        var res = responses.Select(v =>
            new ValueTuple<HttpStatusCode, HttpContent>(v.statusCode,
                new StringContent(JsonConvert.SerializeObject(ResponseDto.Create(v.dto)), Encoding.UTF8,
                    "application/json"))).ToList();

        var client = GetHttpClientWithHttpMessageHandlerSequenceResponseMock(res);

        CommunicationMock.Setup(x => x.HttpClient).Returns(client);
    }

    public Mock<ICommunication> CommunicationMock { get; private set; }
    public Mock<IFeature> FeatureMock { get; private set; }
    public Mock<IGui> GuiMock { get; private set; }
    public Mock<IMedia> MediaMock { get; private set; }
    public Mock<ISharing> SharingMock { get; private set; }
    public Mock<IStorage> StorageMock { get; private set; }

    public void Dispose()
    {
        CommunicationMock = new Mock<ICommunication>();
        FeatureMock = new Mock<IFeature>();
        GuiMock = new Mock<IGui>();
        MediaMock = new Mock<IMedia>();
        SharingMock = new Mock<ISharing>();
        StorageMock = new Mock<IStorage>();
    }

    public T GetService<T>() where T : class
    {
        var collection = new ServiceCollection();
        collection.AddWebApiService();

        collection.AddSingleton(CommunicationMock.Object);
        collection.AddSingleton(FeatureMock.Object);
        collection.AddSingleton(GuiMock.Object);
        collection.AddSingleton(MediaMock.Object);
        collection.AddSingleton(SharingMock.Object);
        collection.AddSingleton(StorageMock.Object);


        using var scope = collection.BuildServiceProvider().CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }

    public async Task Authenticate(string username = CoreTestHelper.Username, string password = CoreTestHelper.Password)
    {
        var service = GetService<IAuthenticationService>();

        await service.Login(LoginRequestDto.Create(username, password));
        await service.UpdateToken("0000");
        await service.UpdateToken();
    }

    private HttpClient GetHttpClientWithHttpMessageHandlerSequenceResponseMock(
        IEnumerable<(HttpStatusCode, HttpContent)> returns)
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

        foreach (var item in returns) handlerPart = AddReturnPart(handlerPart, item.Item1, item.Item2);
        handlerMock.Verify();

        var httpClient = new HttpClient(handlerMock.Object);
        return httpClient;
    }

    private ISetupSequentialResult<Task<HttpResponseMessage>>
        AddReturnPart(ISetupSequentialResult<Task<HttpResponseMessage>> handlerPart,
            HttpStatusCode statusCode, HttpContent content)
    {
        return handlerPart

            // prepare the expected response of the mocked http call
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode, // HttpStatusCode.Unauthorized,
                Content = content //new StringContent("[{'id':1,'value':'1'}]"),
            });
    }

    private static string GetLocalIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return "http://" + ip + ":5000/";
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}