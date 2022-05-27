// using System;
// using Microsoft.Extensions.DependencyInjection;
// using Moq;
// using WeeControl.Application.EssentialContext;
// using WeeControl.Domain.Essential.Entities;
// using WeeControl.SharedKernel.Essential.DataTransferObjects;
// using WeeControl.User.UserApplication.Enums;
// using WeeControl.User.UserApplication.Services;
// using WeeControl.WebApi;
// using Xunit;
//
// namespace WeeControl.User.ServiceCore.Test.Integration.Essential.Essentials.UserServices;
//
// public class RegisterTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
// {
//     private readonly CustomWebApplicationFactory<Startup> factory;
//     private DeviceServiceMock deviceMock;
//
//     public RegisterTests(CustomWebApplicationFactory<Startup> factory)
//     {
//         this.factory = factory;
//         deviceMock = new DeviceServiceMock(typeof(RegisterTests).Namespace);
//     }
//
//     public void Dispose()
//     {
//         deviceMock = null;
//     }
//
//     [Fact]
//     public async void WhenNewUserRegisterWithValidData_ReturnSuccess()
//     {
//         await new UserService(
//                 deviceMock.GetObject(factory.CreateClient())).RegisterAsync(RegisterDtoV1.Create("email@email.com", "username", "password"));
//         
//         deviceMock.StorageMock.Verify(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()));
//         deviceMock.NavigationMock.Verify(x => x.NavigateToAsync(PagesEnum.Home, It.IsAny<bool>()), Times.Once);
//     }
//         
//     [Theory]
//     [InlineData("", "", "")]
//     [InlineData("", "username", "password")]
//     [InlineData("email@email.com", "", "password")]
//     [InlineData("email@email.com", "username", "")]
//     [InlineData("emil", "username", "password")]
//     [InlineData("email@email.com", "us", "password")]
//     [InlineData("email@email.com", "username", "pas")]
//     public async void WhenNewUserRegisterWithInValidData_DisplayDeveloperInvalidData(string email, string username, string password)
//     {
//         await new UserService(
//                 deviceMock.GetObject(factory.CreateClient())).RegisterAsync(RegisterDtoV1.Create(email, username, password));
//             
//
//         deviceMock.AlertMock.Verify(x => 
//             x.DisplayAlert(AlertEnum.DeveloperInvalidUserInput), Times.Once);
//     }
//         
//     [Fact]
//     public async void WhenNewUserRegisterExistingEmail_ReturnConflict()
//     {
//         var user = (Email: "exist@exist.com", Username: "exist", Password: "password", Device: typeof(LoginTests).Namespace);
//         var client = factory.WithWebHostBuilder(builder =>
//             {
//                 builder.ConfigureServices(services =>
//                 {
//                     using var scope = services.BuildServiceProvider().CreateScope();
//                     var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
//                     db.Users.Add(UserDbo.Create(user.Email, user.Username, user.Password));
//                     db.SaveChanges();
//                 });
//             })
//             .CreateClient();
//
//         await new UserService(
//                 deviceMock.GetObject(client)).RegisterAsync(RegisterDtoV1.Create(user.Email, "username", "password"));
//             
//         deviceMock.AlertMock.Verify(x => 
//             x.DisplayAlert(AlertEnum.ExistingEmailOrUsernameExist), Times.Once);
//         
//         await new UserService(
//             deviceMock.GetObject(client)).RegisterAsync(RegisterDtoV1.Create("someemail@email.com", user.Username, "password"));
//             
//         deviceMock.AlertMock.Verify(x => 
//             x.DisplayAlert(AlertEnum.ExistingEmailOrUsernameExist), Times.Exactly(2));
//     }
// }