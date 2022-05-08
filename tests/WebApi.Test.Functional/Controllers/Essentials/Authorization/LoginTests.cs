using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.FunctionalService.BoundedContexts.Authorization;
using WeeControl.Common.FunctionalService.Enums;
using WeeControl.Common.FunctionalService.Interfaces;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.Controllers.Essentials.Authorization
{
    public class LoginTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable, ITestsNotRequireAuthentication
    {
        #region static
        public static async Task<string> LoginAsync(HttpClient client, string username, string password, string device)
        {
            var token = string.Empty;

            
            
            var userDeviceMock = new Mock<IUserDevice>();
            userDeviceMock.SetupAllProperties();
            userDeviceMock.Setup(x => x.DeviceId).Returns(device);
            
            var userCommunicationMock = new Mock<IUserCommunication>();
            userCommunicationMock.SetupAllProperties();
            userCommunicationMock.Setup(x => x.ServerBaseAddress).Returns("http://localhost.com/");
            userCommunicationMock.Setup(x => x.HttpClient)
                .Returns(client);
            
            var userStorageMockMock = new Mock<IUserStorage>();
            userStorageMockMock.SetupAllProperties();
            userStorageMockMock.Setup(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()))
                .Callback((UserDataEnum en, string tkn) => token = tkn);
        
            var response = 
                await new UserOperation(userDeviceMock.Object, userCommunicationMock.Object, userStorageMockMock.Object)
                    .LoginAsync(new LoginDto(username, password));
            
            Assert.True(response.IsSuccess);
            userStorageMockMock.Verify(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()));
            
            return token;
        }
        #endregion
        
        private readonly CustomWebApplicationFactory<Startup> factory;
        private Mock<IUserDevice> userDeviceMock;
        private Mock<IUserCommunication> userCommunicationMockMock;
        private Mock<IUserStorage> userStorageMockMock;
        private LoginDto loginDto = new LoginDto("admin", "admin");

        public LoginTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            userDeviceMock = new Mock<IUserDevice>();
            userDeviceMock.SetupAllProperties();
            userDeviceMock.Setup(x => x.DeviceId).Returns(nameof(LoginTests));

            userCommunicationMockMock = new Mock<IUserCommunication>();
            userCommunicationMockMock.SetupAllProperties();
            userCommunicationMockMock.Setup(x => x.ServerBaseAddress).Returns("http://localhost.com/");

            userStorageMockMock = new Mock<IUserStorage>();
            userStorageMockMock.SetupAllProperties();
        }

        public void Dispose()
        {
            userDeviceMock = null;
            userCommunicationMockMock = null;
            userStorageMockMock = null;
        }

        [Fact]
        public async void WhenSendingInvalidRequest_HttpResponseIsBadRequest()
        {
            // Mock<IUserDevice> deviceMock = new();
            // deviceMock.SetupAllProperties();
            // deviceMock.Setup(x => x.DeviceId).Returns(device);
            //
            // IUserOperation service = new UserOperation(factory.CreateClient(), deviceMock.Object);
            // var response2 = await service.RequestNewToken(null);
            //
            // Assert.Equal(HttpStatusCode.BadRequest, response2.StatuesCode);
        }

        [Fact]
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
        {
            var client = factory.CreateClient();
            
            var token = await LoginAsync(client, "admin", "admin", "device");
            
            Assert.NotEmpty(token);
        }
        
        [Fact]
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode2()
        {
            userCommunicationMockMock.Setup(x => x.HttpClient)
                .Returns(factory.CreateClient());
        
            var response = 
                await new UserOperation(userDeviceMock.Object, userCommunicationMockMock.Object, userStorageMockMock.Object)
                    .LoginAsync(loginDto);
            
            Assert.True(response.IsSuccess);
            userStorageMockMock.Verify(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()));
        }
        
        [Fact]
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode3()
        {
            var userMock = factory.GetUserDeviceMock(nameof(WhenSendingValidRequest_HttpResponseIsSuccessCode));
            var commMock = factory.GetUserCommunicationMock(factory.CreateClient());
            var storageMock = factory.GetUserStorageMockMock();            
            
            var response = 
                await new UserOperation(
                        userMock.Object, 
                        commMock.Object, 
                        storageMock.Object)
                    .LoginAsync(new LoginDto("admin", "admin"));
            
            Assert.True(response.IsSuccess);
            storageMock.Verify(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()));
        }
        
        private HttpClient GetHttpClientForTesting(HttpStatusCode statusCode, HttpContent content)
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = statusCode, 
                Content = content
            };
        
            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
        
            return new HttpClient(httpMessageHandlerMock.Object);
        }
    }
}