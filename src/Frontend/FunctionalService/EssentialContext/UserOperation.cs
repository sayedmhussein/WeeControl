using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WeeControl.Frontend.FunctionalService.Enums;
using WeeControl.Frontend.FunctionalService.Interfaces;
using WeeControl.Frontend.FunctionalService.Models;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.RequestDTOs;
using WeeControl.SharedKernel.Essential.ResponseDTOs;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.RequestsResponses;

namespace WeeControl.Frontend.FunctionalService.EssentialContext;

public class UserOperation : IUserOperation
{
    private readonly IUserDevice userDevice;
    private readonly IUserCommunication userCommunication;
    private readonly IUserStorage userStorage;
    private readonly IDisplayAlert alert;

    public UserOperation(IEssentialUserDevice device, IDisplayAlert alert)
    {
        this.userDevice = device;
        this.userCommunication = device;
        this.userStorage = device;
        this.alert = alert;
    }

    public async Task<IResponseDto> RegisterAsync(RegisterDto loginDto)
    {
        var requestDto = new RequestDto<RegisterDto>(userDevice.DeviceId, loginDto);

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(RegisterDto.HttpPostMethod.AbsoluteUri(userCommunication.ServerBaseAddress)),
            Version = new Version(RegisterDto.HttpPostMethod.Version),
            Method = HttpMethod.Post,
            Content = RequestDto.BuildHttpContentAsJson(requestDto)
        };

        var response = await userCommunication.HttpClient.SendAsync(message);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDto>>();
                var token = responseDto?.Payload?.Token;
                await userStorage.SaveAsync(UserDataEnum.Token, token);
                return ResponseToUi.Accepted(response.StatusCode);
            case HttpStatusCode.Conflict:
                return ResponseToUi.Rejected(response.StatusCode, "Either username or password already exist!");
            default:
                return ResponseToUi.Rejected(response.StatusCode, "Unexpected error occured, error code: " + response.StatusCode);
        }
    }

    public async Task<IResponseDto> LoginAsync(LoginDto loginDto)
    {
        var requestDto = new RequestDto<LoginDto>(userDevice.DeviceId, loginDto);

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(TokenDto.HttpPostMethod.AbsoluteUri(userCommunication.ServerBaseAddress)),
            Version = new Version(TokenDto.HttpPostMethod.Version),
            Method = HttpMethod.Post,
            Content = RequestDto.BuildHttpContentAsJson(requestDto)
        };

        var response = await userCommunication.HttpClient.SendAsync(message);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDto>>();
                var token = responseDto?.Payload?.Token;
                await userStorage.SaveAsync(UserDataEnum.Token, token);
                await userStorage.SaveAsync(UserDataEnum.FullName, responseDto?.Payload?.FullName);
                await userStorage.SaveAsync(UserDataEnum.PhotoUrl, responseDto?.Payload?.PhotoUrl);
                return ResponseToUi.Accepted(response.StatusCode);
            case HttpStatusCode.NotFound:
                return ResponseToUi.Rejected(response.StatusCode, "Invalid username or password!");
            default:
                return ResponseToUi.Rejected(response.StatusCode, "Unexpected error occured, error code: " + response.StatusCode);
        }
    }

    public async Task<IResponseDto> GetTokenAsync()
    {
        var requestDto = new RequestDto(userDevice.DeviceId);

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(TokenDto.HttpPutMethod.AbsoluteUri(userCommunication.ServerBaseAddress)),
            Version = new Version(TokenDto.HttpPutMethod.Version),
            Method = HttpMethod.Put,
            Content = RequestDto.BuildHttpContentAsJson(requestDto)
        };
            
        userCommunication.HttpClient.DefaultRequestHeaders.Clear();
        userCommunication.HttpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Brear", await userStorage.GetAsync(UserDataEnum.Token));
            
        var response = await userCommunication.HttpClient.SendAsync(message);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
            case HttpStatusCode.Accepted:
                var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDto>>();
                var token = responseDto?.Payload?.Token;
                await userStorage.SaveAsync(UserDataEnum.Token, token);
                await userStorage.SaveAsync(UserDataEnum.FullName, responseDto?.Payload?.FullName);
                await userStorage.SaveAsync(UserDataEnum.PhotoUrl, responseDto?.Payload?.PhotoUrl);
                return ResponseToUi.Accepted(response.StatusCode);
            case HttpStatusCode.Forbidden:
                return ResponseToUi.Rejected(response.StatusCode, "Please login again.");
            default:
                return ResponseToUi.Rejected(response.StatusCode, "Unexpected error occured, error code: " + response.StatusCode);
        }
    }

    public async Task<IResponseDto> LogoutAsync()
    {
        await userStorage.ClearAsync();
            
        var requestDto = new RequestDto(userDevice.DeviceId);

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(TokenDto.HttpDeleteMethod.AbsoluteUri(userCommunication.ServerBaseAddress)),
            Version = new Version(TokenDto.HttpDeleteMethod.Version),
            Method = HttpMethod.Delete,
            Content = RequestDto.BuildHttpContentAsJson(requestDto)
        };

        await UpdateAuthorization();

        var response = await userCommunication.HttpClient.SendAsync(message);

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

    public async Task<IResponseDto> UpdatePasswordAsync(PasswordSetForgottenDto loginSetForgottenDto)
    {
        var requestDto = new RequestDto<PasswordSetForgottenDto>(userDevice.DeviceId, loginSetForgottenDto);

        HttpRequestMessage message = new()
        {
            RequestUri = new Uri(PasswordSetForgottenDto.HttpPatchMethod.AbsoluteUri(userCommunication.ServerBaseAddress)),
            Version = new Version(PasswordSetForgottenDto.HttpPatchMethod.Version),
            Method = HttpMethod.Patch,
            Content = RequestDto.BuildHttpContentAsJson(requestDto)
        };

        await UpdateAuthorization();

        var response = await userCommunication.HttpClient.SendAsync(message);

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

    public Task<IResponseDto> ForgotPasswordAsync(PasswordResetRequestDto passwordResetRequestDto)
    {
        throw new NotImplementedException();
    }

    private async Task UpdateAuthorization()
    {
        userCommunication.HttpClient.DefaultRequestHeaders.Clear();
        userCommunication.HttpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Brear", await userStorage.GetAsync(UserDataEnum.Token));
    }
}