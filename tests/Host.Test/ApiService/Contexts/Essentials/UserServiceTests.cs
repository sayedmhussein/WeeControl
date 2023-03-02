using System.Net;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.Domain.Contexts.Essentials;
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
    public async void WhenInvalidDto_ThrowException()
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IUserService>();
        
        await Assert.ThrowsAsync<EntityModelValidationException>(() => 
            service.AddUser(new UserProfileDto()));
    }
    
    [Fact]
    public async void WhenSuccess_DisplayMessageAndNavigateToHome()
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IUserService>(HttpStatusCode.OK);

        var dto = GetUserProfileDto();
        
        await service.AddUser(dto);

        helper.GuiMock
            .Verify(x => 
                x.NavigateToAsync(ApplicationPages.Essential.UserEmailValidationPage, It.IsAny<bool>()), Times.Once);
    }
    
    [Fact]
    public async void WhenInvalidDublicateUser_ReturnNotAcceptable()
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IUserService>(HttpStatusCode.NotAcceptable);

        var dto = GetUserProfileDto();
        
        await service.AddUser(dto);
        
        helper.GuiMock.Verify(x=>x.DisplayAlert(It.IsAny<string>()), Times.Once);
        
        helper.GuiMock
            .Verify(x => 
                x.NavigateToAsync(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.Never);
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
                NationalityCode = "EGP", DateOfBirth = DateOnly.MaxValue
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