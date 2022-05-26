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
    private readonly IAlertService alert;

    public UserService(IDevice device, IServerService server, IAlertService alert)
    {
        this.device = device;
        this.server = server;
        this.alert = alert;
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
                await alert.DisplayAsync(AlertEnum.ExistingEmailOrUsernameExist);
                break;
            case HttpStatusCode.BadRequest:
                await alert.DisplayAsync(AlertEnum.DeveloperInvalidUserInput);
                break;
            default:
                await alert.DisplayAsync(AlertEnum.DeveloperMinorBug);
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
                    await alert.DisplayAsync(AlertEnum.DeveloperInvalidUserInput);
                }
                
                break;
            case HttpStatusCode.NotFound:
                await alert.DisplayAsync(AlertEnum.InvalidUsernameOrPassword);
                break;
            case HttpStatusCode.Forbidden:
                await alert.DisplayAsync(AlertEnum.AccountIsLocked);
                break;
            default:
                await alert.DisplayAsync(AlertEnum.DeveloperInvalidUserInput);
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
                await alert.DisplayAsync(AlertEnum.SessionIsExpiredPleaseLoginAgain);
                await device.Navigation.NavigateToAsync(Pages.Authentication.Login, forceLoad: true);
                break;
            case HttpStatusCode.BadGateway:
                break;
            default:
                await alert.DisplayAsync(AlertEnum.DeveloperMinorBug);
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
                await alert.DisplayAsync(AlertEnum.DeveloperMinorBug);
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
                await alert.DisplayAsync(AlertEnum.PasswordUpdatedSuccessfully);
                await device.Navigation.NavigateToAsync(Pages.Home.Index);
                break;
            case HttpStatusCode.NotFound:
                await alert.DisplayAsync(AlertEnum.InvalidPassword);
                break;
            default:
                await alert.DisplayAsync(AlertEnum.DeveloperMinorBug);
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
        await alert.DisplayAsync(AlertEnum.NewPasswordSent);
    }
}