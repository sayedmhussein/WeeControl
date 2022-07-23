using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Frontend.ApplicationService.Essential.ViewModels;

public class AuthorizationViewModel : ViewModelBase
{
    private readonly IDevice device;
    private readonly IServerOperation server;

    #region Properties
    [Required]
    [StringLength(45, MinimumLength = 3)]
    public string UsernameOrEmail { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    #endregion
    
    public AuthorizationViewModel(IDevice device, IServerOperation server)
    {
        this.device = device;
        this.server = server;
    }

    public async Task Init()
    {
        if (await server.IsTokenValid())
        {
            await device.Navigation.NavigateToAsync(Pages.Shared.IndexPage);
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
    
    public async Task LogoutAsync()
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Authorization.Route)),
            Version = new Version("1.0"),
            Method = HttpMethod.Delete,
        };

        var response = await server.Send(message, new object());

        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
            case HttpStatusCode.BadGateway:
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.Unauthorized:
                await device.Navigation.NavigateToAsync(Pages.Essential.Authentication.LoginPage, forceLoad: true);
                break;
            default:
                await device.Alert.DisplayAlert("AlertEnum.DeveloperMinorBug");
                break;
        }

        await device.Security.DeleteTokenAsync();
    }

    public Task NavigateToRegisterPage()
    {
        return device.Navigation.NavigateToAsync(Pages.Essential.User.RegisterPage);
    }
    
    public Task NavigateToForgotMyPasswordPage()
    {
        return device.Navigation.NavigateToAsync(Pages.Essential.User.ForgotMyPasswordPage);
    }

    private async Task ProcessLoginCommand()
    {
        var response = await server.Send(
            new HttpRequestMessage
            {
                RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Authorization.Route)),
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
                if (await server.IsTokenValid())
                {
                    await device.Navigation.NavigateToAsync(Pages.Shared.IndexPage, forceLoad: true);
                    return;
                }

                await device.Navigation.NavigateToAsync(Pages.Essential.Authentication.LoginPage, forceLoad: true);
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