using System.Net;
using Moq;
using WeeControl.Backend.WebApi;
using WeeControl.Frontend.FunctionalService.Enums;
using WeeControl.Frontend.FunctionalService.EssentialContext;
using Xunit;

namespace WeeControl.Test.WebApi.Test.Functional.Controllers.Essentials.Authorization
{
    public class LogoutTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public LogoutTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
        {
            var client = factory.CreateClient();
            
            var mocks = ApplicationMocks.GetMocks(client, typeof(LogoutTests).Namespace);
            var token = await GetTokenTests.GetRefreshedTokenAsync(client, "admin", "admin", typeof(LogoutTests).Namespace);
            mocks.userStorage.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);

            var response = 
                await new UserOperation(
                        mocks.userDevice.Object, 
                        mocks.userCommunication.Object, 
                        mocks.userStorage.Object)
                    .LogoutAsync();
            
            Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
        }

        [Fact]
        public async void WhenUnAuthenticatedUser_HttpResponseIsUnauthorized()
        {
            var userMock = ApplicationMocks.GetUserDeviceMock(nameof(WhenSendingValidRequest_HttpResponseIsSuccessCode));
            var commMock = ApplicationMocks.GetUserCommunicationMock(factory.CreateClient());
            var storageMock = ApplicationMocks.GetUserStorageMockMock();            
            
            var response = 
                await new UserOperation(
                        userMock.Object, 
                        commMock.Object, 
                        storageMock.Object)
                    .LogoutAsync();
            
            Assert.Equal(HttpStatusCode.Unauthorized, response.HttpStatusCode);
        }

        [Fact]
        public async void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden()
        {
            var client = factory.CreateClient();
            
            var mocks = ApplicationMocks.GetMocks(client, typeof(LogoutTests).Namespace);
            var token = await GetTokenTests.GetRefreshedTokenAsync(client, "admin", "admin", typeof(LogoutTests).Namespace);
            mocks.userStorage.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);
            
            var response1 = 
                await new UserOperation(
                        mocks.userDevice.Object, 
                        mocks.userCommunication.Object, 
                        mocks.userStorage.Object)
                    .LogoutAsync();
            
            var response2 = 
                await new UserOperation(
                        mocks.userDevice.Object, 
                        mocks.userCommunication.Object, 
                        mocks.userStorage.Object)
                    .LogoutAsync();
            Assert.Equal(HttpStatusCode.Forbidden, response2.HttpStatusCode);
        }
    }
}