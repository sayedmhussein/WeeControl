using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.ViewModels.User;

public class RegisterViewModel : ViewModelBase
{
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

    public RegisterViewModel(IDevice device) : base(device)
    {
        this.device = device;
    }

    public async Task RegisterAsync()
    {
        IsLoading = true;
        await ProcessRegister(RegisterDtoV1.Create(Email, Username, Password));
        IsLoading = false;
    }

    private async Task ProcessRegister(RegisterDtoV1 dto)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Base)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
        };

        var response = await SendMessageAsync(message, dto);
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var dto_ = await GetObjectFromJsonResponseAsync<ResponseDto<TokenDtoV1>>(response);
                var token = dto_?.Payload?.Token;
                await device.Security.UpdateTokenAsync(token);
                await device.Navigation.NavigateToAsync(Pages.Home.Index, forceLoad: true);
                break;
            case HttpStatusCode.Conflict:
                await device.Alert.DisplayAlert("AlertEnum.ExistingEmailOrUsernameExist");
                break;
            case HttpStatusCode.BadRequest:
                await device.Alert.DisplayAlert("AlertEnum.DeveloperInvalidUserInput");
                break;
            default:
                await device.Alert.DisplayAlert("AlertEnum.DeveloperMinorBug");
                break;
        }
    }
}