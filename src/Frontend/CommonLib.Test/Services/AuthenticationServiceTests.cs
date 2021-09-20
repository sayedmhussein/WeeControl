using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Moq;
using Newtonsoft.Json;
using WeeControl.Common.SharedKernel.DtosV1;
using WeeControl.Common.SharedKernel.DtosV1.Employee;
using WeeControl.Frontend.CommonLib.Interfaces;
using WeeControl.Frontend.CommonLib.Test.TestHelpers;
using Xunit;

namespace WeeControl.Frontend.CommonLib.Test.Services
{
    public class AuthenticationServiceTests
    {
        private readonly IDevice device;
        
        public AuthenticationServiceTests()
        {
            var localStorageMock = new Mock<ILocalStorage>();
            localStorageMock.SetupAllProperties();
            
            var deviceMock = new Mock<IDevice>();
            deviceMock.Setup(x => x.DeviceId).Returns(typeof(AuthenticationServiceTests).Namespace);
            deviceMock.Setup(x => x.LocalStorage).Returns(localStorageMock.Object);
            device = deviceMock.Object;
        }

        [Fact]
        public async void WhenLoginWithCorrectDto_ReturnOk()
        {
            var expectedResponse = new ResponseDto<EmployeeTokenDto>(new EmployeeTokenDto() {});
            
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponse), Encoding.UTF8, "application/json") 
            });
            var fakeHttpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = new Uri("http://good.com/")};

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(fakeHttpClient);

            // var service = new AuthenticationService(httpClientFactoryMock.Object, device);
            // var response = await service.Login(new CreateLoginDto());
            //
            // Assert.Equal(200, response.HttpStatuesCode);
        }
    }
}