using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using WeeControl.Frontend.ApplicationService.Essential.Legacy;
using WeeControl.Frontend.ApplicationService.Essential.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;

namespace WeeControl.Frontend.ApplicationService.Essential.ViewModels;

public class PasswordChangeLegacyViewModel : LegacyViewModelBase
{
    private readonly IDevice device;
    
    public PasswordChangeModel Model { get; }

    [Required(ErrorMessage = "Old Password is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Password length is between 3 and 50 letters.")]
    [DataType(DataType.Password)]
    [DisplayName("Old Password")]
    public string OldPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Password length is between 6 and 50 letters.")]
    [DataType(DataType.Password)]
    [DisplayName("New Password")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword))]
    [NotMapped]
    public string ConfirmNewPassword { get; set; } = string.Empty;

    public PasswordChangeLegacyViewModel(IDevice device) : base(device)
    {
        this.device = device;
        Model = new PasswordChangeModel();
    }

    public async Task ChangeMyPassword()
    {
        if (string.IsNullOrWhiteSpace(OldPassword) ||
            string.IsNullOrWhiteSpace(NewPassword) ||
            string.IsNullOrWhiteSpace(ConfirmNewPassword) ||
            NewPassword != ConfirmNewPassword)
        {
            await device.Alert.DisplayAlert("Invalid Properties");
            return;
        }

        IsLoading = true;
        await ProcessChangingPassword(SetNewPasswordDtoV1.Create(OldPassword, NewPassword));
        IsLoading = false;
    }

    private async Task ProcessChangingPassword(SetNewPasswordDtoV1? dto)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.ResetPasswordEndPoint)),
            Version = new Version("1.0"),
            Method = HttpMethod.Patch,
        };
        
        var response = await SendMessageAsync(message, dto);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                await device.Alert.DisplayAlert("PasswordUpdatedSuccessfully");
                await device.Navigation.NavigateToAsync(Pages.Shared.IndexPage);
                break;
            case HttpStatusCode.NotFound:
                await device.Alert.DisplayAlert("InvalidPassword");
                break;
            default:
                await device.Alert.DisplayAlert("DeveloperMinorBug");
                break;
        }
    }
}