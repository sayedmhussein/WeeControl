using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WeeControl.Common.BoundedContext.Credentials;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;
using WeeControl.Common.SharedKernel.Enums;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.RequestsResponses;
using IUserOperation = WeeControl.Common.BoundedContext.Credentials.IUserOperation;

namespace WeeControl.Frontend.ServiceLibrary.Operations
{
    public class CredentialsOperation : IUserOperation
    {
        private readonly IUserDevice userDevice;
        private readonly IUserCommunication userCommunication;
        
        public CredentialsOperation(IUserDevice userDevice, IUserCommunication userCommunication)
        {
            this.userDevice = userDevice;
            this.userCommunication = userCommunication;
        }

        public async Task RegisterAsync(RegisterDto loginDto)
        {
            var requestDto = new RequestDto<RegisterDto>() { Payload = loginDto, DeviceId = userDevice.DeviceId };

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(CredentialsLink.Register.Absolute(userCommunication.ServerBaseAddress)),
                Version = new Version(CredentialsLink.Register.Version),
                Method = CredentialsLink.Register.Method,
                Content = RequestDto.BuildHttpContentAsJson(requestDto)
            };

            var response = await userCommunication.HttpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
                return;
            
            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDto>>();
            return;
        }

        public async Task<(HttpStatusCode, string Token)> LoginAsync(LoginDto loginDto)
        {
            var requestDto = new RequestDto<LoginDto>() { Payload = loginDto, DeviceId = userDevice.DeviceId };

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(CredentialsLink.Login.Absolute(userCommunication.ServerBaseAddress)),
                Version = new Version(CredentialsLink.Login.Version),
                Method = CredentialsLink.Login.Method,
                Content = RequestDto.BuildHttpContentAsJson(requestDto)
            };

            var response = await userCommunication.HttpClient.SendAsync(message);
            
            var token = string.Empty;
            
            if (response.IsSuccessStatusCode)
            {
                var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDto>>();
                token = responseDto?.Payload?.Token;
            }

            return (response.StatusCode, token);
            
            //await device.SaveTokenAsync(responseDto?.Payload?.Token);
            //return response.StatusCode;
        }

        public async Task GetTokenAsync()
        {
            var requestDto = new RequestDto() { DeviceId = userDevice.DeviceId };

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(CredentialsLink.RequestRefreshToken.Absolute(userCommunication.ServerBaseAddress)),
                Version = new Version(CredentialsLink.RequestRefreshToken.Version),
                Method = CredentialsLink.RequestRefreshToken.Method,
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
            throw new NotImplementedException();
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
