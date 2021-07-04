using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Common.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;

namespace WeeControl.User.Employee.Test.UnitTests.ViewModels
{
    public abstract class ViewModelTestBase : IDisposable
    {
        protected readonly IApiDicts commonValues;

        protected Mock<IDevice> deviceMock;
        protected Mock<ILogger> loggerMock;

        public ViewModelTestBase()
        {
            commonValues = new ApiDicts();

            deviceMock = new Mock<IDevice>();
            deviceMock.Setup(x => x.Metadata).Returns(new RequestMetadata() { Device = "DeviceUnitTests" });
            deviceMock.Setup(x => x.Internet).Returns(true);
            deviceMock.SetupProperty(x => x.Token);
            deviceMock.Setup(x => x.Token).Returns("storedtoken");
            deviceMock.SetupProperty(x => x.FullUserName);

            loggerMock = new Mock<ILogger>();
        }

        public void Dispose()
        {
            deviceMock = null;
            loggerMock = null;
        }

        protected HttpClient GetNewHttpClient(HttpMessageHandler handler)
        {
            var client = new HttpClient(handler);
            client.BaseAddress = new Uri( commonValues.ApiRoute[ApiRouteEnum.Base]);
            client.DefaultRequestVersion = new Version(commonValues.ApiRoute[ApiRouteEnum.Version]);
            client.Timeout = TimeSpan.FromSeconds(3);

            return client;
        }

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
    }
}
