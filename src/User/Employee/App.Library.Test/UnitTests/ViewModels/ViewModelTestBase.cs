using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using MySystem.SharedKernel.EntityV1Dtos.Common;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.Services;
using MySystem.User.Employee.Services;

namespace MySystem.User.Employee.Test.UnitTests.ViewModels
{
    public class ViewModelTestBase : IDisposable
    {
        protected readonly ISharedValues sharedValues;

        protected Mock<IDevice> deviceMock;
        protected Mock<ILogger> loggerMock;
        protected Mock<IClientServices> clientServicesMock;

        public ViewModelTestBase()
        {
            sharedValues = new SharedValues();

            deviceMock = new Mock<IDevice>();
            deviceMock.Setup(x => x.Metadata).Returns(new RequestMetadata() { Device = "DeviceUnitTests" });
            deviceMock.Setup(x => x.Internet).Returns(true);
            deviceMock.Setup(x => x.DeviceId).Returns(nameof(MySystem.User.Employee.Test.UnitTests.ViewModels) + "-Tests");
            deviceMock.SetupProperty(x => x.Token);
            deviceMock.Setup(x => x.Token).Returns("storedtoken");
            deviceMock.SetupProperty(x => x.FullUserName);

            loggerMock = new Mock<ILogger>();

            clientServicesMock = new Mock<IClientServices>();
            clientServicesMock.Setup(x => x.Device).Returns(deviceMock.Object);
            clientServicesMock.Setup(x => x.Logger).Returns(loggerMock.Object);
            clientServicesMock.Setup(x => x.SharedValues).Returns(sharedValues);
            clientServicesMock.Setup(x => x.SystemUnderTest).Returns(true);
            clientServicesMock.Setup(x => x.AppDataPath).Returns("");
            clientServicesMock.Setup(x => x.Settings).Returns(Employee.Configuration.Config.GetInstance());
        }

        public void Dispose()
        {
            deviceMock = null;
            loggerMock = null;
            clientServicesMock = null;
        }

        protected HttpClient GetNewHttpClient(HttpMessageHandler handler)
        {
            var client = new HttpClient(handler);
            client.BaseAddress = new Uri( sharedValues.ApiRoute[SharedKernel.Enumerators.ApiRouteEnum.Base]);
            client.DefaultRequestVersion = new Version(sharedValues.ApiRoute[SharedKernel.Enumerators.ApiRouteEnum.Version]);
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
