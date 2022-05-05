using System.Net.Http.Headers;
using System.Net.Http.Json;
using WeeControl.Common.BoundedContext.Credentials;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.RequestsResponses;
using IUserOperation = WeeControl.Common.BoundedContext.Credentials.IUserOperation;

namespace WeeControl.Frontend.ServiceLibrary.Operations
{
    public class CredentialsOperation : IUserOperation
    {
        private readonly IUserDevice device;
        
        public CredentialsOperation(IUserDevice device)
        {
            this.device = device;
        }

        public async Task RegisterAsync(RegisterDto loginDto)
        {
            var requestDto = new RequestDto<RegisterDto>() { Payload = loginDto, DeviceId = device.DeviceId };

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(CredentialsLink.Register.Absolute(device.ServerBaseAddress)),
                Version = new Version(CredentialsLink.Register.Version),
                Method = CredentialsLink.Register.Method,
                Content = RequestDto.BuildHttpContentAsJson(requestDto)
            };

            var response = await device.HttpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
                return;
            
            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDto>>();
            return;
        }

        public async Task LoginAsync(LoginDto loginDto)
        {
            var requestDto = new RequestDto<LoginDto>() { Payload = loginDto, DeviceId = device.DeviceId };

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(CredentialsLink.Login.Absolute(device.ServerBaseAddress)),
                Version = new Version(CredentialsLink.Login.Version),
                Method = CredentialsLink.Login.Method,
                Content = RequestDto.BuildHttpContentAsJson(requestDto)
            };

            var response = await device.HttpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
                return;

            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDto>>();
            await device.SaveTokenAsync(responseDto?.Payload?.Token);
            return;
        }

        public async Task GetTokenAsync()
        {
            var requestDto = new RequestDto() { DeviceId = device.DeviceId };

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(CredentialsLink.RequestRefreshToken.Absolute(device.ServerBaseAddress)),
                Version = new Version(CredentialsLink.RequestRefreshToken.Version),
                Method = CredentialsLink.RequestRefreshToken.Method,
                Content = RequestDto.BuildHttpContentAsJson(requestDto)
            };
        
            device.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await device.GetTokenAsync());
            var response = await device.HttpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
                return;

            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDto>>();
            await device.SaveTokenAsync(responseDto?.Payload?.Token);
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
