using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.DataTransferObjects;
using WeeControl.SharedKernel.DataTransferObjects.User;
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
        await ProcessPasswordReset(ForgotMyPasswordDtoV1.Create(Email, Username));
        IsLoading = false;
    }

    private async Task ProcessPasswordReset(ForgotMyPasswordDtoV1 dtoV1)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.ResetPassword)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
        };
        
        var responseMessage = await SendMessageAsync(message, dtoV1);
        if (responseMessage.IsSuccessStatusCode)
        {
            await device.Navigation.NavigateToAsync(Pages.Authentication.LoginPage);
            await device.Alert.DisplayAlert("AlertEnum.NewPasswordSent");
            return;
        }
        
        Console.WriteLine("Invalid message");
        await device.Alert.DisplayAlert("Something went wrong!");
    }
}