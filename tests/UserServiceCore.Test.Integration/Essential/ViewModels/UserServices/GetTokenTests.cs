// using System;
// using System.Net.Http;
// using System.Threading.Tasks;
// using Microsoft.Extensions.DependencyInjection;
// using Moq;
// using WeeControl.Application.EssentialContext;
// using WeeControl.Domain.Essential.Entities;
// using WeeControl.SharedKernel.Services;
// using WeeControl.User.UserServiceCore.Enums;
// using WeeControl.User.UserServiceCore.Services;
// using WeeControl.WebApi;
// using Xunit;
//
// namespace WeeControl.User.ServiceCore.Test.Integration.Essential.Essentials.UserServices;
//
// public class GetTokenTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
// {
//     #region static
//     public static async Task<string> GetRefreshedTokenAsync(HttpClient client, string username, string password,
//         string device)
//     {
//         var token1 = await LoginTests.LoginAsync(client, username, password, device);
//         var token2 = string.Empty;
//             
//         var mocks = new DeviceServiceMock(device);
//         mocks.StorageMock.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token1);
//         mocks.StorageMock.Setup(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()))
//             .Callback((UserDataEnum en, string tkn) => token2 = tkn);
//
//         await new UserService(mocks.GetObject(client))
//                 .GetTokenAsync();
//         
//         Assert.NotEmpty(token2);
//         
//         return token2;
//     }
//     #endregion
//     
//     private HttpClient client;
//     private readonly string deviceName = nameof(GetTokenTests);
//     private DeviceServiceMock deviceMock;
//     private readonly (string Email, string Username, string Password, string Device) user = 
//         (Email: "test@test.test", Username: "test", Password: "test", Device: typeof(GetTokenTests).Namespace);
//
//     public GetTokenTests(CustomWebApplicationFactory<Startup> factory)
//     {
//         client = factory.WithWebHostBuilder(builder =>
//             {
//                 builder.ConfigureServices(services =>
//                 {
//                     using var scope = services.BuildServiceProvider().CreateScope();
//                     var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
//                     db.Users.Add(UserDbo.Create(user.Email, user.Username, new PasswordSecurity().Hash(user.Password)));
//                     db.SaveChanges();
//                 });
//             })
//             .CreateClient();
//
//         deviceMock = new DeviceServiceMock(deviceName);
//     }
//
//     public void Dispose()
//     {
//         client = null;
//         deviceMock = null;
//     }
//         
//     [Fact]
//     public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
//     {
//         var token = await LoginTests.LoginAsync(client, user.Email, user.Password, deviceName);
//         deviceMock.StorageMock.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);
//
//         await new UserService(deviceMock.GetObject(client))
//                 .GetTokenAsync();
//             
//         deviceMock.StorageMock.Verify(x => x.
//             SaveAsync(UserDataEnum.Token, It.IsAny<string>()));
//     }
//
//     [Fact]
//     public async void WhenSendingValidRequest_HttpResponseIsSuccessCode2()
//     {
//         var token = await GetRefreshedTokenAsync(client, user.Username, user.Password, typeof(GetTokenTests).Namespace);
//             
//         Assert.NotEmpty(token);
//     }
//         
//     [Fact]
//     public async void WhenUnAuthenticatedUserOrAuthorized_DisplayLoginAgain()
//     {
//         await new UserService(deviceMock.GetObject(client))
//             .GetTokenAsync();
//             
//         deviceMock.NavigationMock.Verify(x => 
//             x.NavigateToAsync(PagesEnum.Login, It.IsAny<bool>()), Times.Once);
//         
//         deviceMock.StorageMock.Verify(x => 
//             x.ClearAsync(), Times.AtLeastOnce);
//     }
//
//     [Fact]
//     public async void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden()
//     {
//         //When different device...
//         var token = await LoginTests.LoginAsync(client, user.Username, user.Password, "some other device");
//         deviceMock.StorageMock.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);
//
//         await new UserService(deviceMock.GetObject(client))
//                 .GetTokenAsync();
//
//         deviceMock.NavigationMock.Verify(x => 
//             x.NavigateToAsync(PagesEnum.Login, It.IsAny<bool>()), Times.Once);
//         
//         deviceMock.StorageMock.Verify(x => 
//             x.ClearAsync(), Times.AtLeastOnce);
//         
//         deviceMock.AlertMock.Verify(x => 
//             x.DisplayAlert(AlertEnum.SessionIsExpiredPleaseLoginAgain), Times.Once);
//     }
// }