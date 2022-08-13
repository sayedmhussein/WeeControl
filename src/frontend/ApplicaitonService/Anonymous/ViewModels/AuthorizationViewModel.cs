using System.Net;
using System.Net.Http.Json;
using WeeControl.Frontend.ApplicationService.Anonymous.Interfaces;
using WeeControl.Frontend.ApplicationService.Anonymous.Models;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Frontend.ApplicationService.Anonymous.ViewModels;

internal class AuthorizationViewModel : ViewModelBase, IAuthorizationViewModel
{
    private readonly IDevice device;
    private readonly IServerOperation server;
    
    public LoginModel LoginModel { get; }

    public AuthorizationViewModel(IDevice device, IServerOperation server)
    {
        this.device = device;
        this.server = server;

        LoginModel = new LoginModel();
    }

    public async Task Init()
    {
        if (await server.IsTokenValid())
        {
            await device.Navigation.NavigateToAsync(Pages.Anonymous.IndexPage);
        }
    }
    
    public async Task Login()
    {
        if (string.IsNullOrWhiteSpace(LoginModel.UsernameOrEmail) || string.IsNullOrWhiteSpace(LoginModel.Password))
        {
            await device.Alert.DisplayAlert("Please enter your username and password then try again.");
            return;
        }

        IsLoading = true;
        await ProcessLoginCommand();
            LoginModel.Password = string.Empty;
        IsLoading = false;
    }
    
    public async Task Logout()
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Routes.Authorization)),
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
                await device.Navigation.NavigateToAsync(Pages.Anonymous.SplashPage, forceLoad: true);
                break;
            default:
                await device.Alert.DisplayAlert("AlertEnum.DeveloperMinorBug");
                break;
        }

        await device.Security.DeleteTokenAsync();
    }

    private async Task ProcessLoginCommand()
    {
        var response = await server.Send(
            new HttpRequestMessage
            {
                RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Routes.Authorization)),
                Version = new Version("1.0"),
                Method = HttpMethod.Post,
                
            }, 
            AuthenticationRequestDto.Create(LoginModel.UsernameOrEmail, LoginModel.Password));

        if (response.IsSuccessStatusCode)
        {
            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<AuthenticationResponseDto>>();
            var token = responseDto?.Payload?.Token;
            if (token is not null)
            {
                await device.Security.UpdateTokenAsync(token);
                await device.Storage.SaveAsync(nameof(AuthenticationResponseDto.FullName), responseDto?.Payload?.FullName ?? string.Empty);
                if (await server.IsTokenValid())
                {
                    await device.Navigation.NavigateToAsync(Pages.Anonymous.IndexPage, forceLoad: true);
                    return;
                }

                await device.Navigation.NavigateToAsync(Pages.Anonymous.LoginPage, forceLoad: true);
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