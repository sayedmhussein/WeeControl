// using System.Net;
// using WeeControl.Common.SharedKernel.RequestsResponses;
// using WeeControl.Frontend.AppService;
// using WeeControl.Frontend.AppService.Contexts.Temporary.Models;
// using WeeControl.Frontend.AppService.Contexts.Temporary.Services;
// using WeeControl.Frontend.AppService.Services;
//
// namespace WeeControl.Frontend.Service.UnitTest.Contexts.Essential;
//
// public class UserServiceTests
// {
//     #region Init()
//     
//     #endregion
//
//     #region Refresh()
//     
//     #endregion
//     
//     #region Register()
//     [Fact]
//     public async void Register_WhenSuccessResponseCode()
//     {
//         using var helper = new TestHelper(nameof(ChangeMyPassword_WhenSuccessAndOk));
//         var content = helper.GetJsonContent(ResponseDto.Create(AuthenticationResponseDto.Create("token", "fullname")));
//         var device = helper.DeviceMock.GetObject(HttpStatusCode.OK, content);
//
//         var service = new UserService(device, new ServerOperationService(device), new PersistedListService(), new UserAuthorizationService(device, new ServerOperationService(device)));
//
//         await service.Register(new CustomerRegisterModel());
//
//         helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()));
//     }
//     
//     [Theory]
//     [InlineData(HttpStatusCode.BadRequest)]
//     [InlineData(HttpStatusCode.BadGateway)]
//     [InlineData(HttpStatusCode.Conflict)]
//     public async void Register_WhenOtherResponseCode(HttpStatusCode code)
//     {
//         using var helper = new TestHelper(nameof(ChangeMyPassword_WhenSuccessAndOk));
//         var device = helper.DeviceMock.GetObject(code, null!);
//
//         var service = new UserService(device, new ServerOperationService(device), new PersistedListService(), new UserAuthorizationService(device, new ServerOperationService(device)));
//
//         await service.Register(new CustomerRegisterModel());
//         
//         helper.DeviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
//         helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()), Times.Never);
//         Assert.False(service.IsLoading);
//     }
//
//     [Theory(Skip = "Later")]
//     [InlineData("", "", "")]
//     [InlineData("email@email.com", "", "")]
//     [InlineData("", "username", "")]
//     [InlineData("", "", "password")]
//     public async void Register_WhenInvalidProperties(string email, string username, string password)
//     {
//         using var helper = new TestHelper(nameof(ChangeMyPassword_WhenSuccessAndOk));
//         var device = helper.DeviceMock.GetObject(HttpStatusCode.OK, null!);
//
//         var service = new UserService(device, new ServerOperationService(device), new PersistedListService(), new UserAuthorizationService(device, new ServerOperationService(device)));
//         
//
//         await service.Register(new CustomerRegisterModel());
//         
//         helper.DeviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
//         helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()), Times.Never);
//         Assert.False(service.IsLoading);
//     }
//     #endregion
//
//     #region RequestPasswordReset()
//     [Fact]
//     public async void RequestPasswordReset_WhenSuccessAndOk()
//     {
//         using var helper = new TestHelper(nameof(ChangeMyPassword_WhenSuccessAndOk));
//         var device = helper.DeviceMock.GetObject(HttpStatusCode.OK, null!);
//
//         var service = new UserService(device, new ServerOperationService(device), new PersistedListService(), new UserAuthorizationService(device, new ServerOperationService(device)));
//
//         await service.RequestPasswordReset(new PasswordResetModel() { Email = "e@e.e", Username = "u"});
//
//         helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(ApplicationPages.Essential.UserPage, It.IsAny<bool>()));
//     }
//     
//     [Fact]
//     public async void RequestPasswordReset_WhenBadRequest()
//     {
//         using var helper = new TestHelper(nameof(ChangeMyPassword_WhenSuccessAndOk));
//         var device = helper.DeviceMock.GetObject(HttpStatusCode.BadRequest, null!);
//
//         var service = new UserService(device, new ServerOperationService(device), new PersistedListService(), new UserAuthorizationService(device, new ServerOperationService(device)));
//         await service.RequestPasswordReset(new PasswordResetModel());
//         
//         helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(ApplicationPages.Essential.UserPage, It.IsAny<bool>()), Times.Never);
//     }
//     
//     [Fact]
//     public async void RequestPasswordReset_ServerCommunication()
//     {
//         using var helper = new TestHelper(nameof(ChangeMyPassword_WhenSuccessAndOk));
//         var device = helper.DeviceMock.GetObject(HttpStatusCode.BadGateway, null!);
//
//         var service = new UserService(device, new ServerOperationService(device), new PersistedListService(), new UserAuthorizationService(device, new ServerOperationService(device)));
//
//         await service.RequestPasswordReset(new PasswordResetModel());
//         
//         helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(ApplicationPages.Essential.UserPage, It.IsAny<bool>()), Times.Never);
//     }
//     
//     [Theory]
//     [InlineData("", "")]
//     [InlineData("   ", "       ")]
//     [InlineData("email@email.com", "")]
//     [InlineData("", "username")]
//     public async void RequestPasswordReset_WhenInvalidProperties(string email, string username)
//     {
//         using var helper = new TestHelper(nameof(ChangeMyPassword_WhenSuccessAndOk));
//         var device = helper.DeviceMock.GetObject(HttpStatusCode.OK, null!);
//
//         var service = new UserService(device, new ServerOperationService(device), new PersistedListService(), new UserAuthorizationService(device, new ServerOperationService(device)));
//         
//         await service.RequestPasswordReset(new PasswordResetModel() { Email = email, Username = username});
//         
//         helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(ApplicationPages.Essential.UserPage, It.IsAny<bool>()), Times.Never);
//         Assert.False(service.IsLoading);
//     }
//     #endregion
//
//     #region ChangeMyPassword()
//     [Fact]
//     public async void ChangeMyPassword_WhenSuccessAndOk()
//     {
//         using var helper = new TestHelper(nameof(ChangeMyPassword_WhenSuccessAndOk));
//         var device = helper.DeviceMock.GetObject(HttpStatusCode.OK, null!);
//
//         var service = new UserService(device, new ServerOperationService(device), new PersistedListService(), new UserAuthorizationService(device, new ServerOperationService(device)));
//
//         await service.ChangeMyPassword(new PasswordChangeModel()
//         {
//             OldPassword = "bla", ConfirmPassword = "bla", NewPassword = "bla"
//         });
//
//         helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()));
//     }
//     
//     [Fact]
//     public async void ChangeMyPassword_WhenBadRequest()
//     {
//         using var helper = new TestHelper(nameof(ChangeMyPassword_WhenSuccessAndOk));
//         var device = helper.DeviceMock.GetObject(HttpStatusCode.BadRequest, null!);
//
//         var service = new UserService(device, new ServerOperationService(device), new PersistedListService(), new UserAuthorizationService(device, new ServerOperationService(device)));
//
//         await service.ChangeMyPassword(new PasswordChangeModel());
//
//         helper.DeviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
//         helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()), Times.Never);
//     }
//     
//     [Fact]
//     public async void ChangeMyPassword_WhenUnauthorized()
//     {
//         using var helper = new TestHelper(nameof(ChangeMyPassword_WhenSuccessAndOk));
//         var device = helper.DeviceMock.GetObject(HttpStatusCode.Unauthorized, null!);
//
//         var service = new UserService(device, new ServerOperationService(device), new PersistedListService(), new UserAuthorizationService(device, new ServerOperationService(device)));
//
//         await service.ChangeMyPassword(new PasswordChangeModel());
//
//         helper.DeviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
//         helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()), Times.Never);
//     }
//     
//     [Fact]
//     public async void ChangeMyPassword_WhenNotFound()
//     {
//         using var helper = new TestHelper(nameof(ChangeMyPassword_WhenSuccessAndOk));
//         var device = helper.DeviceMock.GetObject(HttpStatusCode.NotFound, null!);
//
//         var service = new UserService(device, new ServerOperationService(device), new PersistedListService(), new UserAuthorizationService(device, new ServerOperationService(device)));
//
//         await service.ChangeMyPassword(new PasswordChangeModel());
//
//         helper.DeviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
//         helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()), Times.Never);
//     }
//     
//     [Fact]
//     public async void ChangeMyPassword_WhenServerCommunicationError()
//     {
//         using var helper = new TestHelper(nameof(ChangeMyPassword_WhenSuccessAndOk));
//         var device = helper.DeviceMock.GetObject(HttpStatusCode.BadGateway, null!);
//
//         var service = new UserService(device, new ServerOperationService(device), new PersistedListService(), new UserAuthorizationService(device, new ServerOperationService(device)));
//
//         await service.ChangeMyPassword(new PasswordChangeModel());
//
//         helper.DeviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
//         helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()), Times.Never);
//     }
//     
//     [Theory]
//     [InlineData("", "", "")]
//     [InlineData("bla", "bla", "")]
//     [InlineData("", "bla", "bla")]
//     [InlineData("bla", "", "bla")]
//     [InlineData("bla", "bla", "notBla")]
//     public async void ChangeMyPassword_WhenInvalidProperties(string oldPassword, string newPassword, string confirmPassword)
//     {
//         using var helper = new TestHelper(nameof(ChangeMyPassword_WhenSuccessAndOk));
//         var device = helper.DeviceMock.GetObject(HttpStatusCode.OK, null!);
//
//         var service = new UserService(device, new ServerOperationService(device), new PersistedListService(), new UserAuthorizationService(device, new ServerOperationService(device)));
//
//         await service.ChangeMyPassword(new PasswordChangeModel()
//         {
//             OldPassword = oldPassword,
//             NewPassword = newPassword,
//             ConfirmPassword = confirmPassword
//         });
//
//         helper.DeviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
//         helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(ApplicationPages.Essential.SplashPage, It.IsAny<bool>()), Times.Never);
//     }
//     #endregion
// }