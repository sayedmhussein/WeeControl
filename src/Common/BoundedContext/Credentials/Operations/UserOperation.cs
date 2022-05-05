using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.RequestsResponses;

namespace WeeControl.Common.BoundedContext.Credentials.Operations
{
    public class UserOperation : IUserOperation
    {
        private readonly IUserDevice device;
        
        public UserOperation(IUserDevice device)
        {
            this.device = device;
        }

        public async Task RegisterAsync(RegisterDto loginDto)
        {
            var requestDto = new RequestDto<RegisterDto>() { Payload = loginDto, DeviceId = device.DeviceId };

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(ApiRouteLink.Register.Absolute(device.ServerBaseAddress)),
                Version = new Version(ApiRouteLink.Register.Version),
                Method = ApiRouteLink.Register.Method,
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
                RequestUri = new Uri(ApiRouteLink.Login.Absolute(device.ServerBaseAddress)),
                Version = new Version(ApiRouteLink.Login.Version),
                Method = ApiRouteLink.Login.Method,
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
                RequestUri = new Uri(ApiRouteLink.RequestRefreshToken.Absolute(device.ServerBaseAddress)),
                Version = new Version(ApiRouteLink.RequestRefreshToken.Version),
                Method = ApiRouteLink.RequestRefreshToken.Method,
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
