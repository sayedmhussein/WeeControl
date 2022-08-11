using System.Net;
using System.Net.Http;
using WeeControl.Frontend.ApplicationService.Essential.Models;
using WeeControl.Frontend.ApplicationService.Essential.ViewModels;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.Frontend.ApplicationService.Services;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Frontend.ApplicationService.UnitTest.ViewModels.User;

public class RegisterViewModelTests : ViewModelTestsBase
{
    public RegisterViewModelTests() : base(nameof(UserViewModel))
    {
    }
    
    [Fact]
    public async void WhenSuccessResponseCode()
    {
        var vm = GetViewModel(Mock.GetObject(HttpStatusCode.OK, GetResponseContent()));

        await vm.RegisterAsync(GetRegisterDto());

        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()));
    }
    
    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.Conflict)]
    public async void WhenOtherResponseCode(HttpStatusCode code)
    {
        var vm =  GetViewModel(Mock.GetObject(code, GetResponseContent()));

        await vm.RegisterAsync(GetRegisterDto());
        
        Mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);
        Assert.False(vm.IsLoading);
    }

    [Theory(Skip = "Later")]
    [InlineData("", "", "")]
    [InlineData("email@email.com", "", "")]
    [InlineData("", "username", "")]
    [InlineData("", "", "password")]
    public async void WhenInvalidProperties(string email, string username, string password)
    {
        var vm = GetViewModel(Mock.GetObject(HttpStatusCode.OK, GetResponseContent()));

        await vm.RegisterAsync(new UserRegisterModel()
        {
            Email = email,
            Username = username,
            Password = password
        });
        
        Mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);
        Assert.False(vm.IsLoading);
    }

    private UserRegisterModel GetRegisterDto()
    {
        return new UserRegisterModel()
        {
            TerritoryId = nameof(IUserModel.TerritoryId),
            FirstName = nameof(IUserModel.FirstName),
            LastName = nameof(IUserModel.LastName),
            Email = nameof(IUserModel.Email) + "@email.com",
            Username = nameof(IUserModel.Username),
            Password = nameof(IUserModel.Password),
            MobileNo = "0123456789",
            Nationality = "TST"
        };
    }

    private HttpContent GetResponseContent()
    {
        var dto =
            ResponseDto.Create<TokenDtoV1>(TokenDtoV1.Create("token", "name", "url"));
        return GetJsonContent(dto);
    }

    private UserViewModel GetViewModel(IDevice device)
    {
        return new UserViewModel(device, new ServerOperationService(device));
    }
}