using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.ViewModels.Essentials;

public class RegisterViewModel : ViewModelBase
{
    private readonly IUserService userService;
    private readonly IDevice device;

    [Required]
    [EmailAddress]
    [DisplayName("Email")]
    public string Email { get; set; }

    [Required]
    [MinLength(3)]
    [StringLength(45, ErrorMessage = "username cannot be longer than 45 characters.")]
    [DisplayName("Username")]
    public string Username { get; set; }

    [Required]
    [MinLength(6)]
    [DataType(DataType.Password)]
    [DisplayName("Password")]
    public string Password { get; set; }

    public RegisterViewModel(IUserService userService, IDevice device)
    {
        this.userService = userService;
        this.device = device;
    }

    public async Task RegisterAsync()
    {
        IsLoading = true;
        await userService.RegisterAsync(RegisterDtoV1.Create(Email, Username, Password));
        IsLoading = false;
    }
}