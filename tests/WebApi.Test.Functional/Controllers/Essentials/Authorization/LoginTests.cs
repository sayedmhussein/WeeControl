using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using WeeControl.Backend.Domain.Databases.Databases.DatabaseObjects.EssentialsObjects;
using WeeControl.Backend.Persistence.BoundedContext.Credentials;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.FunctionalService.BoundedContexts.Authorization;
using WeeControl.Common.FunctionalService.BoundedContexts.Authorization.UiResponsObjects;
using WeeControl.Common.FunctionalService.Enums;
using WeeControl.Common.FunctionalService.Interfaces;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User;
using Xunit;
using Xunit.Sdk;

namespace WeeControl.Backend.WebApi.Test.Functional.Controllers.Essentials.Authorization
{
    public class LoginTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        #region static
        public static async Task<string> LoginAsync(HttpClient client, string username, string password, string device)
        {
            var token = string.Empty;

            var userMock = ApplicationMocks.GetUserDeviceMock(device);
            var commMock = ApplicationMocks.GetUserCommunicationMock(client);
            var storageMock = ApplicationMocks.GetUserStorageMockMock();    
            storageMock.Setup(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()))
                .Callback((UserDataEnum en, string tkn) => token = tkn);
            
            var response = 
                await new UserOperation(
                        userMock.Object, 
                        commMock.Object, 
                        storageMock.Object)
                    .LoginAsync(new LoginDto(username, password));
            
            Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
            Assert.NotEmpty(token);
            
            return token;
        }

        private static Task<LoginResponse> LoginDebugAsync(HttpClient client, string username, string password, string device)
        {
            var userMock = ApplicationMocks.GetUserDeviceMock(device);
            var commMock = ApplicationMocks.GetUserCommunicationMock(client);
            var storageMock = ApplicationMocks.GetUserStorageMockMock();

            var response = new UserOperation(
                        userMock.Object, 
                        commMock.Object, 
                        storageMock.Object)
                    .LoginAsync(new LoginDto(username, password));

            return response;
        }
        #endregion
        
        private readonly CustomWebApplicationFactory<Startup> factory;

        public LoginTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", "password")]
        [InlineData("username", "")]
        public async void WhenSendingInvalidRequest_HttpResponseIsBadRequest(string username, string password)
        {
            var client = factory.CreateClient();
            
            var response = await LoginDebugAsync(client, username, password, nameof(WhenSendingInvalidRequest_HttpResponseIsBadRequest));
            
            Assert.Equal(HttpStatusCode.BadRequest, response.HttpStatusCode);
        }
        
        [Fact]
        public async void WhenUserNotExist_HttpResponseIsNotFound()
        {
            var client = factory.CreateClient();
            
            var response = await LoginDebugAsync(client, "unknown", "unknown", nameof(WhenSendingInvalidRequest_HttpResponseIsBadRequest));
            
            Assert.Equal(HttpStatusCode.NotFound, response.HttpStatusCode);
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
                    .LoginAsync(new LoginDto("admin", "admin"));
            
            Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
            Assert.True(response.IsSuccess);
            storageMock.Verify(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()));
        }
        
        [Fact]
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode2()
        {
            var client = factory.CreateClient();
            
            var token = await LoginAsync(client, "admin", "admin", nameof(WhenSendingValidRequest_HttpResponseIsSuccessCode2));
            
            Assert.NotEmpty(token);
        }

        [Fact]
        public async void Bla()
        {
            var scope = factory.Server.Host.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EssentialDbContext>();

            await db.Users.AddAsync(new UserDbo() { Username = "bla", Password = "bla"});
            await db.SaveChangesAsync();
            
            var client = factory.CreateClient();
            
            var token = await LoginAsync(client, "bla", "bla", nameof(WhenSendingValidRequest_HttpResponseIsSuccessCode2));
            
            Assert.NotEmpty(token);
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