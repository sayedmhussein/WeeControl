using System.Net;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Essential.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.ViewModels.User;

namespace WeeControl.User.UserApplication.Test.ViewModels.User;

public class RegisterViewModelTests : ViewModelTestsBase
{
    public RegisterViewModelTests() : base(nameof(RegisterViewModel))
    {
    }
    
    [Fact]
    public async void WhenSuccessResponseCode()
    {
        var vm = new RegisterViewModel(Mock.GetObject(HttpStatusCode.OK, GetResponseContent()), GetRegisterDto());

        await vm.RegisterAsync();

        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.IndexPage, It.IsAny<bool>()));
    }
    
    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.Conflict)]
    public async void WhenOtherResponseCode(HttpStatusCode code)
    {
        var vm = new RegisterViewModel(Mock.GetObject(code, GetResponseContent()), GetRegisterDto());

        await vm.RegisterAsync();
        
        Mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.IndexPage, It.IsAny<bool>()), Times.Never);
        Assert.False(vm.IsLoading);
    }

    [Theory]
    [InlineData("", "", "")]
    [InlineData("email@email.com", "", "")]
    [InlineData("", "username", "")]
    [InlineData("", "", "password")]
    public async void WhenInvalidProperties(string email, string username, string password)
    {
        var vm = new RegisterViewModel(Mock.GetObject(HttpStatusCode.OK, GetResponseContent()))
        {
            Email = email,
            Username = username,
            Password = password
        };

        await vm.RegisterAsync();
        
        Mock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        Mock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.IndexPage, It.IsAny<bool>()), Times.Never);
        Assert.False(vm.IsLoading);
    }

    private IRegisterDtoV1 GetRegisterDto()
    {
        return new RegisterDtoV1()
        {
            TerritoryId = nameof(IRegisterDtoV1.TerritoryId),
            FirstName = nameof(IRegisterDtoV1.FirstName),
            LastName = nameof(IRegisterDtoV1.LastName),
            Email = nameof(IRegisterDtoV1.Email) + "@email.com",
            Username = nameof(IRegisterDtoV1.Username),
            Password = nameof(IRegisterDtoV1.Password),
            MobileNo = "0123456789"
        };
    }

    private HttpContent GetResponseContent()
    {
        var dto =
            ResponseDto.Create<TokenDtoV1>(TokenDtoV1.Create("token", "name", "url"));
        return GetJsonContent(dto);
    }
}