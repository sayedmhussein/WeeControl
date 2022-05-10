using System.Net;
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
            var userMock = ApplicationMocks.GetUserDeviceMock(nameof(WhenSendingValidRequest_HttpResponseIsSuccessCode));
            var commMock = ApplicationMocks.GetUserCommunicationMock(factory.CreateClient());
            var storageMock = ApplicationMocks.GetUserStorageMockMock();            
            
            var response = 
                await new UserOperation(
                        userMock.Object, 
                        commMock.Object, 
                        storageMock.Object)
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
            
            Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
        }

        [Fact]
        public async void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden()
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
            
            Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
        }
    }
}