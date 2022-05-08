using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeeControl.Common.FunctionalService.BoundedContexts.Authorization.UiResponsObjects;
using WeeControl.Common.FunctionalService.Enums;
using WeeControl.Common.FunctionalService.Interfaces;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User;
using WeeControl.Common.SharedKernel.RequestsResponses;

namespace WeeControl.Common.FunctionalService.BoundedContexts.Authorization
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

        public async Task RegisterAsync(RegisterDto loginDto)
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

            if (!response.IsSuccessStatusCode)
                return;
            
            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDto>>();
            return;
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
                    return LoginResponse.Success();
                case HttpStatusCode.NotFound:
                    return LoginResponse.Failed("Invalid username or password!");
                default:
                    return LoginResponse.Failed("Unexpected error occured, error code: " + response.StatusCode);
            }
        }

        public async Task GetTokenAsync()
        {
            var requestDto = new RequestDto() { DeviceId = userDevice.DeviceId };

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(AuthorizationLink.RequestRefreshToken.Absolute(userCommunication.ServerBaseAddress)),
                Version = new Version(AuthorizationLink.RequestRefreshToken.Version),
                Method = AuthorizationLink.RequestRefreshToken.Method,
                Content = RequestDto.BuildHttpContentAsJson(requestDto)
            };
        
            //userCommunication.HttpClient.DefaultRequestHeaders.Authorization =
                //new AuthenticationHeaderValue("Bearer", await userCommunication.GetAsync(UserDataEnum.Token));
            var response = await userCommunication.HttpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
                return;

            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDto>>();
            //await userDevice.SaveAsync(UserDataEnum.Token, responseDto?.Payload?.Token);
            responseDto.StatuesCode = response.StatusCode;
            return;
        }

        public Task LogoutAsync()
        {
            return Task.CompletedTask;
            //throw new NotImplementedException();
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
    }
}
