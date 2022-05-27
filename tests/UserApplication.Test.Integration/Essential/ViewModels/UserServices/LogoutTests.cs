// using System.Net.Http;
// using Microsoft.Extensions.DependencyInjection;
// using Moq;
// using WeeControl.Application.EssentialContext;
// using WeeControl.Domain.Essential.Entities;
// using WeeControl.SharedKernel.Services;
// using WeeControl.User.UserApplication.Enums;
// using WeeControl.User.UserApplication.Services;
// using WeeControl.WebApi;
// using Xunit;
//
// namespace WeeControl.User.ServiceCore.Test.Integration.Essential.Essentials.UserServices;
//
// public class LogoutTests : IClassFixture<CustomWebApplicationFactory<Startup>>, System.IDisposable
// {
//     private HttpClient client;
//     private DeviceServiceMock deviceMock;
//     private readonly (string Email, string Username, string Password, string Device) user = (Email: "test@test.test", Username: "test", Password: "test", Device: typeof(LogoutTests).Namespace);
//
//     public LogoutTests(CustomWebApplicationFactory<Startup> factory)
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
//         deviceMock = new DeviceServiceMock(nameof(LoginTests));
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
//         var token = await GetTokenTests.GetRefreshedTokenAsync(client, user.Username, user.Password, nameof(LoginTests));
//         deviceMock.StorageMock.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);
//
//         await new UserService(
//                     deviceMock.GetObject(client))
//                 .LogoutAsync();
//             
//         deviceMock.NavigationMock.Verify(x => 
//             x.NavigateToAsync(PagesEnum.Login, It.IsAny<bool>()), Times.Once);
//         
//         deviceMock.StorageMock.Verify(x => 
//             x.ClearAsync(), Times.AtLeastOnce);
//     }
// }