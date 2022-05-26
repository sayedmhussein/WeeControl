using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.ViewModels.Authentication;

public class ForgotMyPasswordViewModel : INotifyPropertyChanged
{
    private readonly IUserService userService;
    private readonly IDevice device;

    [Required]
    [MaxLength(45)]
    [EmailAddress]
    [DisplayName("Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username length is between 3 and 50 letters.")]
    [DisplayName("Username")]
    public string Username { get; set; } = string.Empty;

    public bool IsLoading { get; private set; } = false;
    
    public ForgotMyPasswordViewModel(IUserService userService, IDevice device)
    {
        this.userService = userService;
        this.device = device;
    }

    public async Task RequestPasswordReset()
    {
        await userService.ForgotPasswordAsync(ForgotMyPasswordDto.Create(Email, Username));
        await device.Navigation.NavigateToAsync(Pages.Authentication.Login);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
}