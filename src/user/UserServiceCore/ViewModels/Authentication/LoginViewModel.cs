using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.ViewModels.Authentication;

public class LoginViewModel : ViewModelBase
{
    private readonly IDevice device;

    #region Properties
    public string CardHeaderString { get; private set; }

    public string UsernameOrEmailString { get; private set; }
    
    [Required]
    [StringLength(45, ErrorMessage = "Username cannot be longer than 45 characters.")]
    public string UsernameOrEmail { get; set; } = string.Empty;

    [Required] 
    [DataType(DataType.Password)]
    [DisplayName("Password")]
    public string Password { get; set; } = string.Empty;
    #endregion
    
    public LoginViewModel(IDevice device) : base(device)
    {
        this.device = device;
        
        SetUpStaticStrings();
    }

    public async Task Init()
    {
        if (await RefreshTokenAsync())
        {
            await device.Navigation.NavigateToAsync(Pages.Home.Index);
        }
    }
    
    public async Task LoginAsync()
    {
        IsLoading = true;
        await ProcessLoginCommand();
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

    private async Task ProcessLoginCommand()
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Session)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
        };

        var response = await SendMessageAsync(message, LoginDtoV1.Create(UsernameOrEmail, Password));

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var responseDto = await GetObjectFromJsonResponseAsync<ResponseDto<TokenDtoV1>>(response);
                var token = responseDto?.Payload?.Token;
                if (token is not null)
                {
                    await device.Storage.SaveAsync(nameof(TokenDtoV1.Token), token);
                    await device.Storage.SaveAsync(nameof(TokenDtoV1.FullName), responseDto?.Payload?.FullName);
                    await device.Storage.SaveAsync(nameof(TokenDtoV1.PhotoUrl), responseDto?.Payload?.PhotoUrl);
                    await device.Navigation.NavigateToAsync(Pages.Home.Index, forceLoad: true);
                }
                else
                {
                    await device.Alert.DisplayAlert("AlertEnum.DeveloperInvalidUserInput");
                }
                
                break;
            case HttpStatusCode.NotFound:
                await device.Alert.DisplayAlert("AlertEnum.InvalidUsernameOrPassword");
                break;
            case HttpStatusCode.Forbidden:
                await device.Alert.DisplayAlert("AlertEnum.AccountIsLocked");
                break;
            default:
                await device.Alert.DisplayAlert("AlertEnum.DeveloperInvalidUserInput");
                break;
        }
    }
}