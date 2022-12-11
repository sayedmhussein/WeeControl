using System.Net;
using System.Net.Http.Json;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.DataTransferObjects.User;
using WeeControl.Common.SharedKernel.RequestsResponses;
using WeeControl.Frontend.AppService.Contexts.Essential.Interfaces;
using WeeControl.Frontend.AppService.Contexts.Essential.Models;
using WeeControl.Frontend.AppService.Interfaces;

namespace WeeControl.Frontend.AppService.Contexts.Essential.Services;

internal class UserAuthorizationService : ViewModelBase, IUserAuthorizationService
{
    private readonly IDevice device;
    private readonly IServerOperation server;

    public UserAuthorizationService(IDevice device, IServerOperation server)
    {
        this.device = device;
        this.server = server;
    }

    public async Task<bool> IsAuthorized()
    {
        return await server.IsTokenValid() && await device.Security.IsAuthenticatedAsync();
    }

    public async Task<bool> Login(LoginModel loginModel)
    {
        if (string.IsNullOrWhiteSpace(loginModel.UsernameOrEmail) || string.IsNullOrWhiteSpace(loginModel.Password))
        {
            await device.Alert.DisplayAlert("Please enter your username and password then try again.");
            return false;
        }

        IsLoading = true;
        var success = await ProcessLoginCommand(loginModel);
        loginModel.Password = string.Empty;
        IsLoading = false;
        return success;
    }

    public async Task<bool> UpdateToken(string? otp)
    {
        if (otp is not null && otp.Length < 4)
        {
            await device.Alert.DisplayAlert("AlertEnum.DeveloperMinorBug");
            return false;
        }
        
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Authorization.Route)),
            Version = new Version("1.0"),
            Method = otp is null ? HttpMethod.Patch : HttpMethod.Put
        };
        
        var response = await server.Send(message, otp);
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<AuthenticationResponseDto>>();
                var token = responseDto?.Payload?.Token;
                if (token is not null)
                {
                    await device.Security.UpdateTokenAsync(token);
                }

                if (otp is not null)
                {
                    await device.Navigation.NavigateToAsync(Pages.Essential.SplashPage);
                }
                return true;
            case HttpStatusCode.NotFound:
                await device.Alert.DisplayAlert("AlertEnum.DeveloperMinorBug");
                break;
            case HttpStatusCode.Unauthorized:
                await device.Alert.DisplayAlert("AlertEnum.DeveloperMinorBug");
                await device.Navigation.NavigateToAsync(Pages.Essential.UserPage);
                await device.Security.DeleteTokenAsync();
                break;
            default:
                await device.Alert.DisplayAlert("AlertEnum.DeveloperMinorBug");
                break;
        }

        return false;
    }

    public async Task<bool> Logout()
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Authorization.Route)),
            Version = new Version("1.0"),
            Method = HttpMethod.Delete
        };

        var response = await server.Send(message, new object());
        
        await device.Security.DeleteTokenAsync();
        await device.Navigation.NavigateToAsync(Pages.Essential.SplashPage);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                return true;
            case HttpStatusCode.NotFound:
            case HttpStatusCode.BadGateway:
            case HttpStatusCode.BadRequest:
            case HttpStatusCode.Unauthorized:
                await device.Navigation.NavigateToAsync(Pages.Essential.SplashPage, forceLoad: true);
                break;
            default:
                await device.Alert.DisplayAlert("AlertEnum.DeveloperMinorBug");
                break;
        }
        
        return false;
    }

    private async Task<bool> ProcessLoginCommand(LoginModel loginModel)
    {
        var response = await server.Send(
            new HttpRequestMessage
            {
                RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.Authorization.Route)),
                Version = new Version("1.0"),
                Method = HttpMethod.Post
            }, 
            AuthenticationRequestDto.Create(loginModel.UsernameOrEmail, loginModel.Password));

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<AuthenticationResponseDto>>();
                var token = responseDto?.Payload?.Token;
                if (token is not null)
                {
                    await device.Security.UpdateTokenAsync(token);
                    await device.Storage.SaveAsync(nameof(AuthenticationResponseDto.FullName), responseDto?.Payload?.FullName ?? string.Empty);
                    if (true) //await server.IsTokenValid())
                    {
                        await device.Navigation.NavigateToAsync(Pages.Essential.OtpPage);
                        return true;
                    }
                }
                await device.Alert.DisplayAlert("AlertEnum.DeveloperInvalidUserInput");
                break;
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

        return false;
    }
}