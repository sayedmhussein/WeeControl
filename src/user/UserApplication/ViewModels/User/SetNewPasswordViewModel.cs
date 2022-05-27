using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.DataTransferObjects;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.UserApplication.ViewModels.User;

public class SetNewPasswordViewModel : ViewModelBase
{
    private readonly IDevice device;

    [Required(ErrorMessage = "Old Password is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Password length is between 3 and 50 letters.")]
    [DataType(DataType.Password)]
    [DisplayName("Old Password")]
    public string OldPassword { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Password length is between 6 and 50 letters.")]
    [DataType(DataType.Password)]
    [DisplayName("New Password")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword))]
    [NotMapped]
    public string ConfirmNewPassword { get; set; }
    
    public bool IsLoading { get; private set; }

    public SetNewPasswordViewModel(IDevice device) : base(device)
    {
        this.device = device;
    }

    public async Task ChangeMyPassword()
    {
        IsLoading = true;
        await ProcessChangingPassword(SetNewPasswordDtoV1.Create(OldPassword, NewPassword));
        IsLoading = false;
    }

    private async Task ProcessChangingPassword(SetNewPasswordDtoV1 dto)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Reset)),
            Version = new Version("1.0"),
            Method = HttpMethod.Patch,
        };
        
        var response = await SendMessageAsync(message, dto);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                await device.Alert.DisplayAlert("AlertEnum.PasswordUpdatedSuccessfully");
                await device.Navigation.NavigateToAsync(Pages.Home.Index);
                break;
            case HttpStatusCode.NotFound:
                await device.Alert.DisplayAlert("AlertEnum.InvalidPassword");
                break;
            default:
                await device.Alert.DisplayAlert("AlertEnum.DeveloperMinorBug");
                break;
        }
    }
}