using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WeeControl.Presentations.ServiceLibrary.Enums;
using WeeControl.Presentations.ServiceLibrary.Interfaces;
using WeeControl.Presentations.ServiceLibrary.Models;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Presentations.ServiceLibrary.EssentialContext;

public class UserOperation : OperationBase, IUserOperation
{
    private readonly IDevice userDevice;
    private readonly IDeviceServerCommunication deviceServerCommunication;
    private readonly IDeviceStorage deviceStorage;
    private readonly IDeviceAlert alert;

    public UserOperation(IDevice device, IDeviceAlert alert) : base(device)
    {
        this.userDevice = device;
        this.deviceServerCommunication = device.DeviceServerCommunication;
        this.deviceStorage = device.DeviceStorage;
        this.alert = alert;
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
                await userDevice.DeviceStorage.SaveAsync(UserDataEnum.Token, token);
                userDevice.DeviceSecurity.UpdateToken(token);
                await userDevice.DevicePageNavigation.NavigateToAsync(PagesEnum.Home);
                break;
            case HttpStatusCode.Conflict:
                await userDevice.DeviceAlert.DisplaySimpleAlertAsync("Either username or password already exist!");
                break;
            default:
                await userDevice.DeviceAlert.DisplaySimpleAlertAsync("Unexpected error occured, error code: " +
                                                                     response.StatusCode);
                break;
        }
    }

    public async Task LoginAsync(LoginDtoV1 loginDtoV1)
    {
        var requestDto = new RequestDto<LoginDtoV1>(userDevice.DeviceId, loginDtoV1);

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(deviceServerCommunication.GetFullAddress(Api.Essential.User.Session)),
            Version = new Version("1.0"),
            Method = HttpMethod.Post,
            Content = RequestDto.BuildHttpContentAsJson(requestDto)
        };

        var response = await deviceServerCommunication.HttpClient.SendAsync(message);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDtoV1>>();
                var token = responseDto?.Payload?.Token;
                await deviceStorage.SaveAsync(UserDataEnum.Token, token);
                await deviceStorage.SaveAsync(UserDataEnum.FullName, responseDto?.Payload?.FullName);
                await deviceStorage.SaveAsync(UserDataEnum.PhotoUrl, responseDto?.Payload?.PhotoUrl);
                return ResponseToUi.Accepted(response.StatusCode);
            case HttpStatusCode.NotFound:
                await alert.DisplaySimpleAlertAsync("Invalid username or password!");
                return ResponseToUi.Rejected(response.StatusCode, "Invalid username or password!");
            default:
                await alert.DisplaySimpleAlertAsync("Unexpected error occured, error code: " + response.StatusCode);
                return ResponseToUi.Rejected(response.StatusCode, "Unexpected error occured, error code: " + response.StatusCode);
        }
    }

    public async Task GetTokenAsync()
    {
        await UpdateAuthorizationAsync();
        
        var requestDto = new RequestDto(userDevice.DeviceId);

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(deviceServerCommunication.GetFullAddress(Api.Essential.User.Session)),
            Version = new Version("1.0"),
            Method = HttpMethod.Put,
            Content = RequestDto.BuildHttpContentAsJson(requestDto)
        };

        var response = await deviceServerCommunication.HttpClient.SendAsync(message);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDtoV1>>();
                var token = responseDto?.Payload?.Token;
                await deviceStorage.SaveAsync(UserDataEnum.Token, token);
                await deviceStorage.SaveAsync(UserDataEnum.FullName, responseDto?.Payload?.FullName);
                await deviceStorage.SaveAsync(UserDataEnum.PhotoUrl, responseDto?.Payload?.PhotoUrl);
                return ResponseToUi.Accepted(response.StatusCode);
            case HttpStatusCode.Forbidden:
                await alert.DisplaySimpleAlertAsync("Please login again!");
                await deviceStorage.ClearAsync();
                return ResponseToUi.Rejected(response.StatusCode, "Please login again.");
            default:
                return ResponseToUi.Rejected(response.StatusCode, "Unexpected error occured, error code: " + response.StatusCode);
        }
    }

    public async Task LogoutAsync()
    {
        await UpdateAuthorizationAsync();
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
                return ResponseToUi.Accepted(response.StatusCode);
            case HttpStatusCode.Forbidden:
                return ResponseToUi.Rejected(response.StatusCode, "Illegal Request!");
            default:
                return ResponseToUi.Rejected(response.StatusCode, "Unexpected error occured, error code: " + response.StatusCode);
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

        await UpdateAuthorizationAsync();

        var response = await deviceServerCommunication.HttpClient.SendAsync(message);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                await alert.DisplaySimpleAlertAsync("Password was updated successfully");
                return ResponseToUi.Accepted(response.StatusCode);
            case HttpStatusCode.NotFound:
                return ResponseToUi.Rejected(response.StatusCode, "Old password is not correct!");
            case HttpStatusCode.Unauthorized:
                return ResponseToUi.Rejected(response.StatusCode, "You are not authorized to to this!");
            default:
                return ResponseToUi.Rejected(response.StatusCode, "Unexpected error occured, error code: " + response.StatusCode);
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
        await alert.DisplaySimpleAlertAsync("You will receive new password, please check your email.");
        return ResponseToUi.Accepted(response.StatusCode);
    }

    private async Task UpdateAuthorizationAsync()
    {
        deviceServerCommunication.HttpClient.DefaultRequestHeaders.Clear();
        deviceServerCommunication.HttpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Brear", await deviceStorage.GetAsync(UserDataEnum.Token));
    }
}