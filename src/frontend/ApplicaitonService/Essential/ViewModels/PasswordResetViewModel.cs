using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;

namespace WeeControl.Frontend.ApplicationService.Essential.ViewModels;

public class PasswordResetViewModel : ViewModelBase
{
    private readonly IDevice device;
    private readonly IServerOperation server;

    [Required]
    [MaxLength(45)]
    [EmailAddress]
    [DisplayName("Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username length is between 3 and 50 letters.")]
    [DisplayName("Username")]
    public string Username { get; set; } = string.Empty;

    public PasswordResetViewModel(IDevice device, IServerOperation server)
    {
        this.device = device;
        this.server = server;
    }

    public async Task RequestPasswordReset()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Username))
        {
            await device.Alert.DisplayAlert("You didn't entered proper data");
            return;
        }
        
        IsLoading = true;
        //await ProcessPasswordReset(ForgotMyPasswordDtoV1.Create(Email, Username));
        IsLoading = false;
    }

    private async Task ProcessPasswordReset(UserPasswordResetRequestDto? dtoV1)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Routes.Customer)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
        };
        
        var responseMessage = await server.Send(message, dtoV1);
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