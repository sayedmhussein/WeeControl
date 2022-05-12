using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Moq;
using WeeControl.Backend.WebApi;
using WeeControl.Common.FunctionalService.Enums;
using WeeControl.Common.FunctionalService.EssentialContext.Authorization;
using Xunit;

namespace WeeControl.test.WebApi.Test.Functional.Controllers.Essentials.Authorization
{
    public class GetTokenTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        #region static
        public static async Task<string> GetRefreshedTokenAsync(HttpClient client, string username, string password,
            string device)
        {
            var token1 = await LoginTests.LoginAsync(client, username, password, device);
            var token2 = string.Empty;
            
            var mocks = ApplicationMocks.GetMocks(client, device);
            mocks.userStorage.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token1);
            mocks.userStorage.Setup(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()))
                .Callback((UserDataEnum en, string tkn) => token2 = tkn);

            var response = 
                await new UserOperation(
                        mocks.userDevice.Object, 
                        mocks.userCommunication.Object, 
                        mocks.userStorage.Object)
                    .GetTokenAsync();
            
            Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
            Assert.NotEmpty(token2);
        
            return token2;
        }
        #endregion
        
        private readonly CustomWebApplicationFactory<Startup> factory;

        public GetTokenTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }
        
        [Fact]
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
        {
            var client = factory.CreateClient();
            
            var mocks = ApplicationMocks.GetMocks(client, typeof(GetTokenTests).Namespace);
            var token = await LoginTests.LoginAsync(client, "admin", "admin", typeof(GetTokenTests).Namespace);
            mocks.userStorage.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);

            var response = 
                await new UserOperation(
                        mocks.userDevice.Object, 
                        mocks.userCommunication.Object, 
                        mocks.userStorage.Object)
                    .GetTokenAsync();
            
            Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
        }

        [Fact]
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode2()
        {
            var token = await GetRefreshedTokenAsync(factory.CreateClient(), "admin", "admin", typeof(GetTokenTests).Namespace);
            
            Assert.NotEmpty(token);
        }
        
        [Fact]
        public async void WhenUnAuthenticatedUser_HttpResponseIsUnauthorized()
        {
            var client = factory.CreateClient();
            
            var mocks = ApplicationMocks.GetMocks(client, typeof(GetTokenTests).Namespace);

            var response = 
                await new UserOperation(
                        mocks.userDevice.Object, 
                        mocks.userCommunication.Object, 
                        mocks.userStorage.Object)
                    .GetTokenAsync();

            Assert.Equal(HttpStatusCode.Unauthorized, response.HttpStatusCode);
        }

        [Fact]
        public async void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden()
        {
            //When different device...
            var client = factory.CreateClient();
            
            var mocks = ApplicationMocks.GetMocks(client, "Some Other Device");
            var token = await LoginTests.LoginAsync(client, "admin", "admin", typeof(GetTokenTests).Namespace);
            mocks.userStorage.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);

            var response = 
                await new UserOperation(
                        mocks.userDevice.Object, 
                        mocks.userCommunication.Object, 
                        mocks.userStorage.Object)
                    .GetTokenAsync();

            Assert.Equal(HttpStatusCode.Forbidden, response.HttpStatusCode);
        }
    }
}
