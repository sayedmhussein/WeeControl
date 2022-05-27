using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.DataTransferObjects;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.UserApplication.ViewModels.User;

public class ForgotMyPasswordViewModel : ViewModelBase
{
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

    public ForgotMyPasswordViewModel(IDevice device) : base(device)
    {
        this.device = device;
    }

    public async Task RequestPasswordReset()
    {
        IsLoading = true;
        await ProcessPasswordReset(ForgotMyPasswordDto.Create(Email, Username));
        await device.Navigation.NavigateToAsync(Pages.Authentication.Login);
    }

    private async Task ProcessPasswordReset(ForgotMyPasswordDto dto)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Reset)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
        };
        
        var response = await SendMessageAsync(message, dto);
        await device.Navigation.NavigateToAsync(Pages.Authentication.Login);
        await device.Alert.DisplayAlert("AlertEnum.NewPasswordSent");
    }
}