using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.ViewModels.Authentication;

public class LoginViewModel : ViewModelBase
{
    private readonly IUserService userService;
    private readonly IDevice device;

    public string CardHeaderString { get; private set; }

    public string UsernameOrEmailString { get; private set; }
    
    [Required]
    [StringLength(45, ErrorMessage = "Username cannot be longer than 45 characters.")]
    public string UsernameOrEmail { get; set; } = string.Empty;

    [Required] 
    [DataType(DataType.Password)]
    [DisplayName("Password")]
    public string Password { get; set; } = string.Empty;

    public LoginViewModel(IUserService userService, IDevice device)
    {
        this.userService = userService;
        this.device = device;
        
        SetUpStaticStrings();
    }

    public async Task Init()
    {
        if (await device.Security.IsAuthenticatedAsync())
        {
            await userService.GetTokenAsync();
            if (await device.Security.IsAuthenticatedAsync())
            {
                await device.Navigation.NavigateToAsync(Pages.Home.Index);
            }
        }
    }
    
    public async Task LoginAsync()
    {
        IsLoading = true;
        await userService.LoginAsync(LoginDtoV1.Create(UsernameOrEmail, Password));
        Password = string.Empty;
        IsLoading = false;
    }

    public Task NavigateToRegisterPage()
    {
        return device.Navigation.NavigateToAsync(Pages.User.Register);
    }
    
    public Task NavigateToForgotMyPasswordPage()
    {
        return device.Navigation.NavigateToAsync(Pages.User.RequestNewPassword);
    }

    private void SetUpStaticStrings()
    {
        CardHeaderString = "Please enter your username and password";
        UsernameOrEmailString = "Username or Email";
    }
}