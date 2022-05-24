using System.Net;
using System.Net.Http.Json;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;
using WeeControl.User.UserServiceCore.InternalHelpers;

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
                device.Security.UpdateToken(token);
                await device.Navigation.NavigateToAsync(PagesEnum.Home, forceLoad: true);
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
                    await device.Navigation.NavigateToAsync(PagesEnum.Home, forceLoad: true);
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
        var requestDto = new RequestDto(device.DeviceId);

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Session)),
            Version = new Version("1.0"),
            Method = HttpMethod.Put,
        };

        var response = await server.SendMessageAsync(message, requestDto);
        
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDtoV1>>();
                var token = responseDto?.Payload?.Token;
                await device.Storage.SaveAsync(UserDataEnum.Token, token);
                await device.Storage.SaveAsync(UserDataEnum.FullName, responseDto?.Payload?.FullName);
                await device.Storage.SaveAsync(UserDataEnum.PhotoUrl, responseDto?.Payload?.PhotoUrl);
                break;
            case HttpStatusCode.Unauthorized:
            case HttpStatusCode.Forbidden:
                await device.Storage.ClearAsync();
                await device.Alert.DisplayAlert(AlertEnum.SessionIsExpiredPleaseLoginAgain);
                await device.Navigation.NavigateToAsync(PagesEnum.Login, forceLoad: true);
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
        await device.Storage.ClearAsync();
            
        var requestDto = new RequestDto(device.DeviceId);

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Session)),
            Version = new Version("1.0"),
            Method = HttpMethod.Delete,
            Content = RequestDto.BuildHttpContentAsJson(requestDto)
        };

        var response = await device.Server.HttpClient.SendAsync(message);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                await device.Navigation.NavigateToAsync(PagesEnum.Login, forceLoad: true);
                break;
            default:
                await device.Alert.DisplayAlert(AlertEnum.DeveloperMinorBug);
                break;
        }
    }

    public async Task UpdatePasswordAsync(SetNewPasswordDtoV1 loginDtoV1)
    {
        var requestDto = new RequestDto<SetNewPasswordDtoV1>(device.DeviceId, loginDtoV1);

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Reset)),
            Version = new Version("1.0"),
            Method = HttpMethod.Patch,
            Content = RequestDto.BuildHttpContentAsJson(requestDto)
        };
        
        var response = await server.SendMessageAsync(message);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                await device.Alert.DisplayAlert(AlertEnum.PasswordUpdatedSuccessfully);
                await device.Navigation.NavigateToAsync(PagesEnum.Home);
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
        var requestDto = new RequestDto<ForgotMyPasswordDto>(device.DeviceId, forgotMyPasswordDto);

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(device.Server.GetFullAddress(Api.Essential.User.Reset)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
            Content = RequestDto.BuildHttpContentAsJson(requestDto)
        };
        
        var response = await server.SendMessageAsync(message);
        await device.Navigation.NavigateToAsync(PagesEnum.Login);
        await device.Alert.DisplayAlert(AlertEnum.NewPasswordSent);
    }
}