using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.UserApplication.ViewModels.Authentication;

public class LoginViewModel : ViewModelBase
{
    private readonly IDevice device;

    #region Properties
    public string CardHeaderLabel { get; private set; } = "Please enter your username and password";
    public string UsernameOrEmailLabel { get; private set; } = "Username or Email";
    public string PasswordLabel { get; private set; } = "Password";
    public string ForgotPasswordButtonLabel { get; private set; } = "Forgot Password";
    public string RegisterButtonLabel { get; private set; } = "Register";
    
    [Required]
    [StringLength(45, MinimumLength = 3)]
    public string UsernameOrEmail { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    #endregion
    
    public LoginViewModel(IDevice device) : base(device)
    {
        this.device = device;
    }

    public async Task Init()
    {
        if (await RefreshTokenAsync())
        {
            await device.Navigation.NavigateToAsync(Pages.Home.IndexPage);
        }
    }
    
    public async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(UsernameOrEmail) || string.IsNullOrWhiteSpace(Password))
        {
            await device.Alert.DisplayAlert("Please enter your username and password then try again.");
            return;
        }

        IsLoading = true;
        await ProcessLoginCommand();
        Password = string.Empty;
        IsLoading = false;
    }

    public Task NavigateToRegisterPage()
    {
        return device.Navigation.NavigateToAsync(Pages.User.RegisterPage);
    }
    
    public Task NavigateToForgotMyPasswordPage()
    {
        return device.Navigation.NavigateToAsync(Pages.User.ForgotMyPasswordPage);
    }

    private async Task ProcessLoginCommand()
    {
        var response = await SendMessageAsync(
            new HttpRequestMessage
            {
                RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Authorization.EndPoint)),
                Version = new Version("1.0"),
                Method = HttpMethod.Post,
            }, 
            LoginDtoV1.Create(UsernameOrEmail, Password));

        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDtoV1>>();
            var token = responseDto?.Payload?.Token;
            if (token is not null)
            {
                await device.Security.UpdateTokenAsync(token);
                await device.Storage.SaveAsync(nameof(TokenDtoV1.FullName), responseDto?.Payload?.FullName ?? string.Empty);
                await device.Storage.SaveAsync(nameof(TokenDtoV1.PhotoUrl), responseDto?.Payload?.PhotoUrl ?? string.Empty);
                if (await RefreshTokenAsync())
                {
                    await device.Navigation.NavigateToAsync(Pages.Home.IndexPage, forceLoad: true);
                    return;
                }

                await device.Navigation.NavigateToAsync(Pages.Authentication.LoginPage, forceLoad: true);
                return;
            }
            else
            {
                await device.Alert.DisplayAlert("AlertEnum.DeveloperInvalidUserInput");
            }

            return;
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                await device.Alert.DisplayAlert("Invalid username or password, please try again.");
                break;
            case HttpStatusCode.Forbidden:
                await device.Alert.DisplayAlert("Your account has been locked, contact the administrator.");
                break;
            default:
                await device.Alert.DisplayAlert("Unexpected error occured! " + response.StatusCode);
                break;
        }
    }
}