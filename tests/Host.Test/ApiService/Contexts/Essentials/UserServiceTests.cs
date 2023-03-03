using System.Net;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.SharedKernel;
using WeeControl.Core.SharedKernel.Contexts.Essentials;
using WeeControl.Core.Test;
using WeeControl.Host.WebApiService;
using WeeControl.Host.WebApiService.Contexts.Essentials;

namespace WeeControl.Host.Test.ApiService.Contexts.Essentials;

public class UserServiceTests
{
    #region AddUser

    [Fact]
    public async void WhenInvalidDto_DisplayMessageAndNeverNavigate()
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IUserService>();

        var dto = GetUserProfileDto();
        dto.Person.FirstName = string.Empty;
        
        await service.AddUser(dto);
        
        helper.GuiMock
            .Verify(x=>x.DisplayAlert(It.IsAny<string>()), Times.Once);
        helper.GuiMock
            .Verify(x => 
                x.NavigateToAsync(ApplicationPages.Essential.OtpPage, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void WhenSuccess_NavigateToOtpPage()
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IUserService>(HttpStatusCode.OK);
        var dto = GetUserProfileDto();
        
        await service.AddUser(dto);

        helper.GuiMock
            .Verify(x => 
                x.NavigateToAsync(ApplicationPages.Essential.OtpPage, It.IsAny<bool>()), Times.Once);
    }
    
    [Fact]
    public async void WhenInvalidSameUserExist_DisplayMessageAndNotNavigate()
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IUserService>(HttpStatusCode.NotAcceptable);
        var dto = GetUserProfileDto();
        
        await service.AddUser(dto);
        
        helper.GuiMock
            .Verify(x=>x.DisplayAlert(It.IsAny<string>()), Times.Once);
        helper.GuiMock
            .Verify(x => 
                x.NavigateToAsync(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }

    private UserProfileDto GetUserProfileDto()
    {
        var contact = new ContactModel() { };
        var contacts = new List<ContactModel>() { };
        
        var dto = new UserProfileDto
        {
            Person =
            {
                FirstName = "Firstname", LastName = "Lastname",
                NationalityCode = "EGP", DateOfBirth = DateTime.MaxValue
            }, User =
            {
                Email = CoreTestHelper.Email,
                Username = CoreTestHelper.Username,
                Password = CoreTestHelper.Password
            }, Contact =
            {
                new ContactModel()
                {
                    ContactType = "Mobile", 
                    ContactValue = CoreTestHelper.MobileNo
                }
            }, Addresses =
            {
                new AddressModel() { }
            }
        };
        
        return dto;
    }
    #endregion
}