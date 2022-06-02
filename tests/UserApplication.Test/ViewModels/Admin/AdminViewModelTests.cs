using System.Net;
using WeeControl.SharedKernel.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.ViewModels.Admin;

namespace WeeControl.User.UserApplication.Test.ViewModels.Admin;

public class AdminViewModelTests : ViewModelTestsBase
{
    public AdminViewModelTests() : base(nameof(AdminListOfUsersViewModel))
    {
    }

    [Fact]
    public async void WhenSuccess()
    {
        var content = GetJsonContent(new ResponseDto<IEnumerable<UserDtoV1>>(new List<UserDtoV1>()));
        var vm = new AdminListOfUsersViewModel(mock.GetObject(HttpStatusCode.OK, content));

        await vm.GetListOfUsers();
    }

    [Theory]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    public async void WhenUnauthorizedOrForbidden(HttpStatusCode code)
    {
        var vm = new AdminListOfUsersViewModel(mock.GetObject(code, null));

        await vm.GetListOfUsers();
        
        Assert.Empty(vm.ListOfUsers);
        mock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.AtLeast(1));
    }
}