using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WeeControl.Common.FunctionalService.Enums;
using WeeControl.Common.FunctionalService.EssentialContext.Authorization.UiResponseObjects;
using WeeControl.Common.FunctionalService.Interfaces;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User;
using WeeControl.Common.SharedKernel.RequestsResponses;

namespace WeeControl.Common.FunctionalService.EssentialContext.Authorization
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

        public async Task<LoginResponse> RegisterAsync(RegisterDto loginDto)
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
                    return LoginResponse.Accepted(response.StatusCode);
                case HttpStatusCode.NotFound:
                    return LoginResponse.Rejected(response.StatusCode, "Please try again!");
                default:
                    return LoginResponse.Rejected(response.StatusCode, "Unexpected error occured, error code: " + response.StatusCode);
            }
        }

        public async Task<LoginResponse> LoginAsync(LoginDto loginDto)
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
                    return LoginResponse.Accepted(response.StatusCode);
                case HttpStatusCode.NotFound:
                    return LoginResponse.Rejected(response.StatusCode, "Invalid username or password!");
                default:
                    return LoginResponse.Rejected(response.StatusCode, "Unexpected error occured, error code: " + response.StatusCode);
            }
        }

        public async Task<LoginResponse> GetTokenAsync()
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
                    return LoginResponse.Accepted(response.StatusCode);
                case HttpStatusCode.Forbidden:
                    return LoginResponse.Rejected(response.StatusCode, "Please login again.");
                default:
                    return LoginResponse.Rejected(response.StatusCode, "Unexpected error occured, error code: " + response.StatusCode);
            }
        }

        public async Task<LogoutResponse> LogoutAsync()
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
                    var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<LogoutResponse>>();
                    return LogoutResponse.Accepted(response.StatusCode);
                case HttpStatusCode.Forbidden:
                    return LogoutResponse.Rejected(response.StatusCode, "Illegal Request!");
                default:
                    return LogoutResponse.Rejected(response.StatusCode, "Unexpected error occured, error code: " + response.StatusCode);
            }
        }

        public Task UpdateEmailAsync(UpdateEmailAsync loginDto)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePasswordAsync(UpdatePasswordDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
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
