﻿using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Sayed.MySystem.ClientService.Services;
using Sayed.MySystem.Shared.Configuration.Models;

namespace Sayed.MySystem.ClientService.Test.Tools
{
    public static class TestMocks
    {
        public static Mock<IDevice> DeviceMock => GetDeviceMock();
        public static Mock<IApi> ApiMock => GetApiMock();
        public static Mock<ILogger> LoggerMock => GetLoggerMock();

        public static Mock<HttpMessageHandler> GetHttpMessageHandlerMock(HttpResponseMessage httpResponseMessage)
        {
            return GetHttpMessageHandlerMock(() => httpResponseMessage);
        }

        public static Mock<HttpMessageHandler> GetHttpMessageHandlerMock(Func<HttpResponseMessage> httpResponseMessage)
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                 .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                 .Returns(Task<HttpResponseMessage>.Factory.StartNew(httpResponseMessage));
            return handler;
        }

        public static Mock<HttpMessageHandler> GetHttpMessageHandlerMock(HttpStatusCode statusCode, HttpContent content)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(statusCode) { Content = content };
            return GetHttpMessageHandlerMock(httpResponseMessage);
        }

        public static Mock<HttpMessageHandler> GetHttpMessageHandlerMock(HttpStatusCode statusCode, string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(statusCode) { Content = content };
            return GetHttpMessageHandlerMock(httpResponseMessage);
        }

        public static Mock<IDevice> GetDeviceMock()
        {
            var device = new Mock<IDevice>();
            device.SetupProperty(x => x.Token, new Random().NextDouble().ToString());
            device.Setup(x => x.Internet).Returns(true);
            device.Setup(x => x.DeviceId).Returns(new Random().NextDouble().ToString());
            return device;
        }

        public static Mock<IClientServices> GetClientServicesMock(IDevice device = null)
        {
            var service = new Mock<IClientServices>();
            service.SetupProperty(p => p.SystemUnderTest, true);
            service.SetupGet(p => p.Device).Returns(device ?? GetDeviceMock().Object);
            return service;
        }

        public static Mock<IApi> GetApiMock()
        {
            var apiMock = new Mock<IApi>();
            apiMock.Setup(p => p.Base).Returns(new Uri("http://bla.com"));
            apiMock.Setup(p => p.Login).Returns("Api");
            apiMock.Setup(p => p.Token).Returns("Api");
            return apiMock;
        }

        public static Mock<ILogger> GetLoggerMock()
        {
            return new Mock<ILogger>();
        }
    }
}
