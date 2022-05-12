using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User;
using WeeControl.Common.SharedKernel.RequestsResponses;
using WeeControl.Frontend.FunctionalService.Enums;
using WeeControl.Frontend.FunctionalService.Interfaces;
using WeeControl.Frontend.FunctionalService.Models;

namespace WeeControl.Frontend.FunctionalService.EssentialContext
{
    public class UserOperation : IUserOperation
    {
        private readonly IUserDevice userDevice;
        private readonly IUserCommunication userCommunication;
        private readonly IUserStorage userStorage;

        public UserOperation(IUserDevice userDevice, IUserCommunication userCommunication, IUserStorage userStorage)
        {
            this.userDevice = userDevice;
            this.userCommunication = userCommunication;
            this.userStorage = userStorage;
        }

        public async Task<IResponseToUi> RegisterAsync(RegisterDto loginDto)
        {
            var requestDto = new RequestDto<RegisterDto>() { Payload = loginDto, DeviceId = userDevice.DeviceId };

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(AuthorizationLink.Register.Absolute(userCommunication.ServerBaseAddress)),
                Version = new Version(AuthorizationLink.Register.Version),
                Method = AuthorizationLink.Register.Method,
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
                case HttpStatusCode.NotFound:
                    return ResponseToUi.Rejected(response.StatusCode, "Please try again!");
                default:
                    return ResponseToUi.Rejected(response.StatusCode, "Unexpected error occured, error code: " + response.StatusCode);
            }
        }

        public async Task<IResponseToUi> LoginAsync(LoginDto loginDto)
        {
            var requestDto = new RequestDto<LoginDto>() { Payload = loginDto, DeviceId = userDevice.DeviceId };

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(AuthorizationLink.Login.Absolute(userCommunication.ServerBaseAddress)),
                Version = new Version(AuthorizationLink.Login.Version),
                Method = AuthorizationLink.Login.Method,
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

        public async Task<IResponseToUi> GetTokenAsync()
        {
            var requestDto = new RequestDto() { DeviceId = userDevice.DeviceId };

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(AuthorizationLink.RequestRefreshToken.Absolute(userCommunication.ServerBaseAddress)),
                Version = new Version(AuthorizationLink.RequestRefreshToken.Version),
                Method = AuthorizationLink.RequestRefreshToken.Method,
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

        public async Task<IResponseToUi> LogoutAsync()
        {
            await userStorage.ClearAsync();
            
            var requestDto = new RequestDto(userDevice.DeviceId);

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(AuthorizationLink.Logout.Absolute(userCommunication.ServerBaseAddress)),
                Version = new Version(AuthorizationLink.Logout.Version),
                Method = AuthorizationLink.Logout.Method,
                Content = RequestDto.BuildHttpContentAsJson(requestDto)
            };

            await UpdateAuthorization();

            var response = await userCommunication.HttpClient.SendAsync(message);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Accepted:
                    var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<ResponseToUi>>();
                    return ResponseToUi.Accepted(response.StatusCode);
                case HttpStatusCode.Forbidden:
                    return ResponseToUi.Rejected(response.StatusCode, "Illegal Request!");
                default:
                    return ResponseToUi.Rejected(response.StatusCode, "Unexpected error occured, error code: " + response.StatusCode);
            }
        }

        public Task<IResponseToUi> UpdateEmailAsync(UpdateEmailAsync loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<IResponseToUi> UpdatePasswordAsync(UpdatePasswordDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<IResponseToUi> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
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
}
