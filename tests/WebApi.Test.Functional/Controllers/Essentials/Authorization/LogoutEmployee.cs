using System.Net;
using System.Net.Http.Headers;
using WeeControl.Backend.WebApi;
using WeeControl.Common.FunctionalService.EssentialContext.Authorization;
using Xunit;

namespace WeeControl.test.WebApi.Test.Functional.Controllers.Essentials.Authorization
{
    public class LogoutEmployee : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public LogoutEmployee(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
        {
            var client = factory.CreateClient();
            var token = await LoginTests.LoginAsync(client, "admin", "admin", typeof(LogoutEmployee).Namespace);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var mocks = ApplicationMocks.GetMocks(client, typeof(LogoutEmployee).Namespace);

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
            var token = await LoginTests.LoginAsync(client, "admin", "admin", typeof(LogoutEmployee).Namespace);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var mocks = ApplicationMocks.GetMocks(client, typeof(LogoutEmployee).Namespace);

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