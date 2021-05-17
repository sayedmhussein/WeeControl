using System;
using Xunit;
using Moq;
using Sayed.MySystem.ClientService.Services;
using Sayed.MySystem.Shared.Configuration.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Moq.Protected;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace Sayed.MySystem.ClientService.UnitTest.Services
{
    public class ClientServiceTesters
    {
        [Fact]
        public void Constructor_WhenDeviceIsNull_ThrowArgumentNullException()
        {
            Action action = new(() => new ClientServices(null, new Mock<IApi>().Object, new Mock<ILogger>().Object));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void Constructor_WhenApiIsNull_ThrowArgumentNullException()
        {
            Action action = new(() => new ClientServices(new Mock<IDevice>().Object, null, new Mock<ILogger>().Object));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void Constructor_WhenLoggerIsNull_ThrowArgumentNullException()
        {
            Action action = new(() => new ClientServices(new Mock<IDevice>().Object, new Mock<IApi>().Object, null));

            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void Properity_AppDataPath_MustNotNullOrEmpty()
        {
            var service = new ClientServices(new Mock<IDevice>().Object, new Mock<IApi>().Object, new Mock<ILogger>().Object);
            Assert.NotNull(service.AppDataPath);
            Assert.NotEmpty(service.AppDataPath);
        }

        [Fact]
        public void Properity_HttpClientInstance_SetUpBaseAddress_ShouldBeSame()
        {
            Uri baseUri = new("http://correct.com");
            var apiMock = new Mock<IApi>();
            apiMock.Setup(x => x.Base).Returns(baseUri);

            var service = new ClientServices(new Mock<IDevice>().Object, apiMock.Object, new Mock<ILogger>().Object, true);

            Assert.Equal(baseUri, service.HttpClientInstance.BaseAddress);
        }

        [Fact]
        public void Properity_HttpClientInstance_IsImmutableProperty()
        {
            Uri correctBaseUri = new("http://correct.com");
            Uri newBaseUri = new("http://new.com");

            var apiMock = new Mock<IApi>();
            apiMock.Setup(x => x.Base).Returns(correctBaseUri);

            var service = new ClientServices(new Mock<IDevice>().Object, apiMock.Object, new Mock<ILogger>().Object, true);
            service.HttpClientInstance.BaseAddress = newBaseUri;

            Assert.NotEqual(newBaseUri, service.HttpClientInstance.BaseAddress);
        }

        [Fact]
        public async void Function_GetResponseAsync_WhenGettingSomeData_ReturnTheData()
        {
            var stringContent = new StringContent("SomeString");

            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                 .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                 .Returns(Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Accepted) { Content = stringContent }));

            var service = new ClientServices(new Mock<IDevice>().Object, new Mock<IApi>().Object, new Mock<ILogger>().Object, handler.Object, true);
            

            var response = await service.GetResponseAsync(HttpMethod.Get, new Uri("http://google.com"));
            response.EnsureSuccessStatusCode();

            Assert.NotEmpty(response.Content.ToString());
            Assert.Equal(await stringContent.ReadAsStringAsync(), await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async void Function_GetResponseAsync_WhenServerReturn500_ResponseShouldBe500()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                 .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                 .Returns(Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.InternalServerError)));

            var service = new ClientServices(new Mock<IDevice>().Object, new Mock<IApi>().Object, new Mock<ILogger>().Object, handler.Object, true);

            var response = await service.GetResponseAsync(HttpMethod.Get, new Uri("http://google.com"));

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async void Function_GetResponseAsync_WhenPostingAnObject_ReturnSameObject()
        {
            var x = new SomeType() { Bla = "Bla" };
            var obj = JsonConvert.SerializeObject(x);
            var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");

            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                 .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                 .Returns(Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = content }));

            var service = new ClientServices(new Mock<IDevice>().Object, new Mock<IApi>().Object, new Mock<ILogger>().Object, handler.Object, true);

            var response = await service.GetResponseAsync(HttpMethod.Post, new Uri("http://google.com"), obj);

            Assert.Equal(x.Bla, (await response.Content.ReadAsAsync<SomeType>()).Bla);
        }

        private class SomeType
        {
            public string Bla { get; set; }
        }
    }
}
