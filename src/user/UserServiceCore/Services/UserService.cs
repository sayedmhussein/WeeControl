using System.Net;
using System.Net.Http.Json;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.UserServiceCore.Services;

public class UserService : BaseService, IUserService
{
    private readonly IDevice userDevice;
    private readonly IDeviceServerCommunication deviceServerCommunication;
    private readonly IDeviceStorage deviceStorage;

    public UserService(IDevice device) : base(device)
    {
        this.userDevice = device;
        this.deviceServerCommunication = device.Server;
        this.deviceStorage = device.Storage;
    }

    public async Task RegisterAsync(RegisterDtoV1 loginDtoV1)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(deviceServerCommunication.GetFullAddress(Api.Essential.User.Base)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
            Content = ConvertObjectToJsonContent(loginDtoV1)
        };

        var response = await SendMessageAsync(message);
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var dto = await GetObjectAsync<ResponseDto<TokenDtoV1>>(response);
                var token = dto?.Payload?.Token;
                await userDevice.Storage.SaveAsync(UserDataEnum.Token, token);
                userDevice.Security.UpdateToken(token);
                await userDevice.Navigation.NavigateToAsync(PagesEnum.Home, forceLoad: true);
                break;
            case HttpStatusCode.Conflict:
                await userDevice.Alert.DisplayAlert(AlertEnum.ExistingEmailOrUsernameExist);
                break;
            case HttpStatusCode.BadRequest:
                await userDevice.Alert.DisplayAlert(AlertEnum.DeveloperInvalidUserInput);
                break;
            default:
                await userDevice.Alert.DisplayAlert(AlertEnum.DeveloperMinorBug);
                break;
        }
    }

    public async Task LoginAsync(LoginDtoV1 loginDtoV1)
    {
        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(deviceServerCommunication.GetFullAddress(Api.Essential.User.Session)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
            Content = ConvertObjectToJsonContent(loginDtoV1)
        };

        var response = await SendMessageAsync(message);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDtoV1>>();
                var token = responseDto?.Payload?.Token;
                await deviceStorage.SaveAsync(UserDataEnum.Token, token);
                await deviceStorage.SaveAsync(UserDataEnum.FullName, responseDto?.Payload?.FullName);
                await deviceStorage.SaveAsync(UserDataEnum.PhotoUrl, responseDto?.Payload?.PhotoUrl);
                await userDevice.Navigation.NavigateToAsync(PagesEnum.Home, forceLoad: true);
                break;
            case HttpStatusCode.NotFound:
                await userDevice.Alert.DisplayAlert(AlertEnum.InvalidUsernameOrPassword);
                break;
            case HttpStatusCode.Forbidden:
                await userDevice.Alert.DisplayAlert(AlertEnum.AccountIsLocked);
                break;
            default:
                await userDevice.Alert.DisplayAlert(AlertEnum.DeveloperInvalidUserInput);
                break;
        }
    }

    public async Task GetTokenAsync()
    {
        var requestDto = new RequestDto(userDevice.DeviceId);

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(deviceServerCommunication.GetFullAddress(Api.Essential.User.Session)),
            Version = new Version("1.0"),
            Method = HttpMethod.Put,
            Content = RequestDto.BuildHttpContentAsJson(requestDto)
        };

        var response = await SendMessageAsync(message);
        
        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDtoV1>>();
                var token = responseDto?.Payload?.Token;
                await deviceStorage.SaveAsync(UserDataEnum.Token, token);
                await deviceStorage.SaveAsync(UserDataEnum.FullName, responseDto?.Payload?.FullName);
                await deviceStorage.SaveAsync(UserDataEnum.PhotoUrl, responseDto?.Payload?.PhotoUrl);
                break;
            case HttpStatusCode.Unauthorized:
            case HttpStatusCode.Forbidden:
                await userDevice.Storage.ClearAsync();
                await userDevice.Alert.DisplayAlert(AlertEnum.SessionIsExpiredPleaseLoginAgain);
                await userDevice.Navigation.NavigateToAsync(PagesEnum.Login, forceLoad: true);
                break;
            case HttpStatusCode.BadGateway:
                break;
            default:
                await userDevice.Alert.DisplayAlert(AlertEnum.DeveloperMinorBug);
                break;
        }
    }

    public async Task LogoutAsync()
    {
        await deviceStorage.ClearAsync();
            
        var requestDto = new RequestDto(userDevice.DeviceId);

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(deviceServerCommunication.GetFullAddress(Api.Essential.User.Session)),
            Version = new Version("1.0"),
            Method = HttpMethod.Delete,
            Content = RequestDto.BuildHttpContentAsJson(requestDto)
        };

        var response = await deviceServerCommunication.HttpClient.SendAsync(message);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                await userDevice.Navigation.NavigateToAsync(PagesEnum.Login, forceLoad: true);
                break;
            default:
                await userDevice.Alert.DisplayAlert(AlertEnum.DeveloperMinorBug);
                break;
        }
    }

    public async Task UpdatePasswordAsync(MeForgotPasswordDtoV1 loginDtoV1)
    {
        var requestDto = new RequestDto<MeForgotPasswordDtoV1>(userDevice.DeviceId, loginDtoV1);

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(deviceServerCommunication.GetFullAddress(Api.Essential.User.Reset)),
            Version = new Version("1.0"),
            Method = HttpMethod.Patch,
            Content = RequestDto.BuildHttpContentAsJson(requestDto)
        };
        
        var response = await deviceServerCommunication.HttpClient.SendAsync(message);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                await userDevice.Alert.DisplayAlert(AlertEnum.PasswordUpdatedSuccessfully);
                await userDevice.Navigation.NavigateToAsync(PagesEnum.Home);
                break;
            case HttpStatusCode.NotFound:
                await userDevice.Alert.DisplayAlert(AlertEnum.InvalidPassword);
                break;
            default:
                await userDevice.Alert.DisplayAlert(AlertEnum.DeveloperMinorBug);
                break;
        }
    }

    public async Task ForgotPasswordAsync(PutNewPasswordDtoV1 putNewPasswordDtoV1)
    {
        await UpdateAuthorizationAsync();
        
        var requestDto = new RequestDto<PutNewPasswordDtoV1>(userDevice.DeviceId, putNewPasswordDtoV1);

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(deviceServerCommunication.GetFullAddress(Api.Essential.User.Reset)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
            Content = RequestDto.BuildHttpContentAsJson(requestDto)
        };
        
        var response = await deviceServerCommunication.HttpClient.SendAsync(message);
        await userDevice.Navigation.NavigateToAsync(PagesEnum.Login);
        await userDevice.Alert.DisplayAlert(AlertEnum.NewPasswordSent);
    }
}