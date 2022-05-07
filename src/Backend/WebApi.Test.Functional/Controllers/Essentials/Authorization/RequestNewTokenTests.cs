using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User;
using WeeControl.Frontend.ServiceLibrary.BoundedContexts.Authorization;
using WeeControl.Frontend.ServiceLibrary.Enums;
using WeeControl.Frontend.ServiceLibrary.Interfaces;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.Controllers.Essentials.Authorization
{
    public class RequestNewTokenTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable, ITestsNotRequireAuthentication
    {
        // #region static
        // public static async Task<string> GetNewTokenAsync(HttpClient client, string username, string password, string device)
        // {
        //     var token = string.Empty;
        //     
        //     // Mock<IUserDevice> deviceMock = new();
        //     // deviceMock.SetupAllProperties();
        //     // deviceMock.Setup(x => x.DeviceId).Returns(device);
        //     // deviceMock.Setup(x => x.SaveTokenAsync(It.IsAny<string>())).Callback<string>(y => token = y);
        //     
        //     // IAuthenticationService service = new AuthenticationService(client, deviceMock.Object);
        //     // var response = await service.RequestNewToken(new LoginDto(username, password));
        //
        //
        //     return token;
        // }
        // #endregion
        
        private readonly CustomWebApplicationFactory<Startup> factory;
        private Mock<IUserDevice> userDeviceMock;
        private Mock<IUserCommunication> userCommunicationMockMock;
        private Mock<IUserStorage> userStorageMockMock;
        private LoginDto loginDto = new("admin", "admin");

        public RequestNewTokenTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            userDeviceMock = new Mock<IUserDevice>();
            userDeviceMock.SetupAllProperties();
            userDeviceMock.Setup(x => x.DeviceId).Returns(nameof(RequestNewTokenTests));

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
            userCommunicationMockMock.Setup(x => x.HttpClient)
                .Returns(factory.CreateClient());
        
            var response = 
                await new UserOperation(userDeviceMock.Object, userCommunicationMockMock.Object, userStorageMockMock.Object)
                    .LoginAsync(loginDto);
            
            Assert.True(response.IsSuccess);
            userStorageMockMock.Verify(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()));
        }
    }
}