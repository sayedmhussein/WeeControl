using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WeeControl.Frontend.ApplicationService.Essential.Legacy;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;

namespace WeeControl.Frontend.ApplicationService.Essential.ViewModels;

public class PasswordResetLegacyViewModel : LegacyViewModelBase
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

    public PasswordResetLegacyViewModel(IDevice device) : base(device)
    {
        this.device = device;
    }

    public async Task RequestPasswordReset()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Username))
        {
            await device.Alert.DisplayAlert("You didn't entered proper data");
            return;
        }
        
        IsLoading = true;
        await ProcessPasswordReset(ForgotMyPasswordDtoV1.Create(Email, Username));
        IsLoading = false;
    }

    private async Task ProcessPasswordReset(ForgotMyPasswordDtoV1? dtoV1)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.ResetPasswordEndPoint)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
        };
        
        var responseMessage = await SendMessageAsync(message, dtoV1);
        if (responseMessage.IsSuccessStatusCode)
        {
            await device.Navigation.NavigateToAsync(Pages.Essential.Authentication.LoginPage);
            await device.Alert.DisplayAlert("New Password was created, please check your email.");
            return;
        }
        
        Console.WriteLine("Invalid message: " + responseMessage.ReasonPhrase);
        await device.Alert.DisplayAlert("Something went wrong!");
    }
}