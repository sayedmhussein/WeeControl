using System;
using Xunit;
using Moq;
using MySystem.Web.ClientService.Services;
using MySystem.Web.Shared.Configuration.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Moq.Protected;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using MySystem.Web.ClientService.Test.Tools;

namespace MySystem.Web.ClientService.Test.Unit.Services
{
    public class ClientServiceTesters
    {
        #region Constructors
        [Fact]
        public void Constructor_WhenDeviceIsNull_ThrowArgumentNullException()
        {
            Action action = new(() => new ClientServices(null, new Mock<IApi>().Object));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void Constructor_WhenApiIsNull_ThrowArgumentNullException()
        {
            Action action = new(() => new ClientServices(new Mock<IDevice>().Object, null));

            Assert.Throws<ArgumentNullException>(action);
        }
        #endregion

        #region Properties
        [Fact]
        public void Properity_AppDataPath_MustNotNullOrEmpty()
        {
            var service = new ClientServices(new Mock<IDevice>().Object, new Mock<IApi>().Object);
            Assert.NotNull(service.AppDataPath);
            Assert.NotEmpty(service.AppDataPath);
        }

        [Fact]
        public void Properity_HttpClientInstance_SetUpBaseAddress_ShouldBeSame()
        {
            Uri baseUri = new("http://correct.com");
            var apiMock = new Mock<IApi>();
            apiMock.Setup(x => x.Base).Returns(baseUri);

            var service = new ClientServices(TestMocks.GetDeviceMock().Object, apiMock.Object, new Mock<ILogger<ClientServices>>().Object);
            service.SystemUnderTest = true;

            Assert.Equal(baseUri, service.HttpClientInstance.BaseAddress);
        }

        [Fact]
        public void Properity_HttpClientInstance_IsImmutableProperty()
        {
            Uri correctBaseUri = new("http://correct.com");
            Uri newBaseUri = new("http://new.com");

            var apiMock = new Mock<IApi>();
            apiMock.Setup(x => x.Base).Returns(correctBaseUri);

            var service = new ClientServices(new Mock<IDevice>().Object, apiMock.Object, new Mock<ILogger<ClientServices>>().Object);
            service.SystemUnderTest = true;

            service.HttpClientInstance.BaseAddress = newBaseUri;

            Assert.NotEqual(newBaseUri, service.HttpClientInstance.BaseAddress);
        }
        #endregion

        #region Functions
        [Fact]
        public async void Function_GetResponseAsync_WhenGettingSomeData_ReturnTheData()
        {
            var stringContent = new StringContent("SomeString");

            var handler = TestMocks.GetHttpMessageHandlerMock(new HttpResponseMessage(HttpStatusCode.Accepted) { Content = stringContent });

            var service = new ClientServices(new Mock<IDevice>().Object, new Mock<IApi>().Object, new Mock<ILogger<ClientServices>>().Object, handler.Object);
            service.SystemUnderTest = true;

            var response = await service.GetResponseAsync(HttpMethod.Get, new Uri("http://google.com"));
            response.EnsureSuccessStatusCode();

            Assert.NotEmpty(response.Content.ToString());
            Assert.Equal(await stringContent.ReadAsStringAsync(), await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async void Function_GetResponseAsync_WhenServerReturn500_ResponseShouldBe500()
        {
            var handler = TestMocks.GetHttpMessageHandlerMock(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            var service = new ClientServices(new Mock<IDevice>().Object, new Mock<IApi>().Object, new Mock<ILogger<ClientServices>>().Object, handler.Object);
            service.SystemUnderTest = true;

            var response = await service.GetResponseAsync(HttpMethod.Get, new Uri("http://google.com"));

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async void Function_GetResponseAsync_WhenPostingAnObject_ReturnSameObject()
        {
            var x = new SomeType() { Bla = "Bla" };
            var obj = JsonConvert.SerializeObject(x);
            var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");

            var handler = TestMocks.GetHttpMessageHandlerMock(new HttpResponseMessage(HttpStatusCode.OK) { Content = content });

            var service = new ClientServices(new Mock<IDevice>().Object, new Mock<IApi>().Object, new Mock<ILogger<ClientServices>>().Object, handler.Object);
            service.SystemUnderTest = true;

            var response = await service.GetResponseAsync(HttpMethod.Post, new Uri("http://google.com"), obj);

            Assert.Equal(x.Bla, (await response.Content.ReadAsAsync<SomeType>()).Bla);
        }
        #endregion

        private class SomeType
        {
            public string Bla { get; set; }
        }
    }
}
