using System.Net;
using System.Net.Http.Json;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.Services;

internal class UserService : IUserService
{
    private readonly IDevice device;
    private readonly IServerService server;

    public UserService(IDevice device, IServerService server)
    {
        this.device = device;
        this.server = server;
    }

    public async Task RegisterAsync(RegisterDtoV1 loginDtoV1)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Base)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
        };

        var response = await server.SendMessageAsync(message, loginDtoV1);
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var dto = await server.GetObjectFromJsonResponseAsync<ResponseDto<TokenDtoV1>>(response);
                var token = dto?.Payload?.Token;
                await device.Storage.SaveAsync(UserDataEnum.Token, token);
                await device.Security.UpdateTokenAsync(token);
                await device.Navigation.NavigateToAsync(Pages.Home.Index, forceLoad: true);
                break;
            case HttpStatusCode.Conflict:
                await device.Alert.DisplayAlert(AlertEnum.ExistingEmailOrUsernameExist);
                break;
            case HttpStatusCode.BadRequest:
                await device.Alert.DisplayAlert(AlertEnum.DeveloperInvalidUserInput);
                break;
            default:
                await device.Alert.DisplayAlert(AlertEnum.DeveloperMinorBug);
                break;
        }
    }

    public async Task LoginAsync(LoginDtoV1 loginDtoV1)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Session)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
        };

        var response = await server.SendMessageAsync(message, loginDtoV1);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var responseDto = await server.GetObjectFromJsonResponseAsync<ResponseDto<TokenDtoV1>>(response);
                var token = responseDto?.Payload?.Token;
                if (token is not null)
                {
                    await device.Storage.SaveAsync(UserDataEnum.Token, token);
                    await device.Storage.SaveAsync(UserDataEnum.FullName, responseDto?.Payload?.FullName);
                    await device.Storage.SaveAsync(UserDataEnum.PhotoUrl, responseDto?.Payload?.PhotoUrl);
                    await device.Navigation.NavigateToAsync(Pages.Home.Index, forceLoad: true);
                }
                else
                {
                    await device.Alert.DisplayAlert(AlertEnum.DeveloperInvalidUserInput);
                }
                
                break;
            case HttpStatusCode.NotFound:
                await device.Alert.DisplayAlert(AlertEnum.InvalidUsernameOrPassword);
                break;
            case HttpStatusCode.Forbidden:
                await device.Alert.DisplayAlert(AlertEnum.AccountIsLocked);
                break;
            default:
                await device.Alert.DisplayAlert(AlertEnum.DeveloperInvalidUserInput);
                break;
        }
    }

    public async Task GetTokenAsync()
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Session)),
            Version = new Version("1.0"),
            Method = HttpMethod.Put
        };

        var response = await server.SendMessageAsync(message, new RequestDto(device.DeviceId));
        
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDtoV1>>();
                var token = responseDto?.Payload?.Token;
                if (responseDto is not null && token is not null)
                {
                    await device.Storage.SaveAsync(UserDataEnum.Token, token);
                    await device.Storage.SaveAsync(UserDataEnum.FullName, responseDto?.Payload?.FullName);
                    await device.Storage.SaveAsync(UserDataEnum.PhotoUrl, responseDto?.Payload?.PhotoUrl);
                    await device.Security.UpdateTokenAsync(token);
                }
                else
                {
                    throw new NullReferenceException("Response DTO from server is null");
                }
                
                break;
            case HttpStatusCode.Unauthorized:
            case HttpStatusCode.Forbidden:
                await device.Security.DeleteTokenAsync();
                await device.Storage.ClearAsync();
                await device.Alert.DisplayAlert(AlertEnum.SessionIsExpiredPleaseLoginAgain);
                await device.Navigation.NavigateToAsync(Pages.Authentication.Login, forceLoad: true);
                break;
            case HttpStatusCode.BadGateway:
                break;
            default:
                await device.Alert.DisplayAlert(AlertEnum.DeveloperMinorBug);
                break;
        }
    }

    public async Task LogoutAsync()
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Session)),
            Version = new Version("1.0"),
            Method = HttpMethod.Delete,
        };

        var response = await server.SendMessageAsync(message, new object());

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                await device.Navigation.NavigateToAsync(Pages.Authentication.Login, forceLoad: true);
                break;
            default:
                await device.Alert.DisplayAlert(AlertEnum.DeveloperMinorBug);
                break;
        }

        await device.Security.DeleteTokenAsync();
    }

    public async Task UpdatePasswordAsync(SetNewPasswordDtoV1 dto)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Reset)),
            Version = new Version("1.0"),
            Method = HttpMethod.Patch,
        };
        
        var response = await server.SendMessageAsync(message, dto);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                await device.Alert.DisplayAlert(AlertEnum.PasswordUpdatedSuccessfully);
                await device.Navigation.NavigateToAsync(Pages.Home.Index);
                break;
            case HttpStatusCode.NotFound:
                await device.Alert.DisplayAlert(AlertEnum.InvalidPassword);
                break;
            default:
                await device.Alert.DisplayAlert(AlertEnum.DeveloperMinorBug);
                break;
        }
    }

    public async Task ForgotPasswordAsync(ForgotMyPasswordDto forgotMyPasswordDto)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Reset)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
        };
        
        var response = await server.SendMessageAsync(message, forgotMyPasswordDto);
        await device.Navigation.NavigateToAsync(Pages.Authentication.Login);
        await device.Alert.DisplayAlert(AlertEnum.NewPasswordSent);
    }
}