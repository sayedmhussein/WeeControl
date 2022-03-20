using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeeControl.Common.BoundedContext.Credentials.DataTransferObjects;
using WeeControl.Common.BoundedContext.Interfaces;
using WeeControl.Common.BoundedContext.RequestsResponses;

namespace WeeControl.Common.BoundedContext.Credentials.Operations
{
    public class UserOperation : IUserOperation
    {
        private readonly IUserDevice device;
        private readonly HttpClient httpClient;

        public UserOperation(IUserDevice device, HttpClient httpClient)
        {
            this.device = device;
            this.httpClient = httpClient;
        }
        
        public UserOperation(IUserDevice device, IHttpClientFactory httpClient)
        {
            this.device = device;
            this.httpClient = httpClient.CreateClient("UnSecured");
        }

        public async Task<IResponseDto<TokenDto>> RegisterAsync(RegisterDto loginDto)
        {
            var requestDto = new RequestDto<RegisterDto>() { Payload = loginDto, DeviceId = device.DeviceId };

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(ApiRouteLink.Register.Absolute(device.ServerBaseAddress)),
                Version = new Version(ApiRouteLink.Register.Version),
                Method = ApiRouteLink.Register.Method,
                Content = RequestDto.BuildHttpContentAsJson(requestDto)
            };

            var response = await httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
                return new ResponseDto<TokenDto>(null) {StatuesCode = response.StatusCode};
            
            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDto>>();
            responseDto.StatuesCode = response.StatusCode;
            return responseDto;
        }

        public async Task<IResponseDto<TokenDto>> LoginAsync(LoginDto loginDto)
        {
            var requestDto = new RequestDto<LoginDto>() { Payload = loginDto, DeviceId = device.DeviceId };

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(ApiRouteLink.Login.Absolute(device.ServerBaseAddress)),
                Version = new Version(ApiRouteLink.Login.Version),
                Method = ApiRouteLink.Login.Method,
                Content = RequestDto.BuildHttpContentAsJson(requestDto)
            };

            var response = await httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
                return new ResponseDto<TokenDto>(null) { StatuesCode = response.StatusCode };

            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDto>>();
            await device.SaveTokenAsync(responseDto?.Payload?.Token);
            responseDto.StatuesCode = response.StatusCode;
            return responseDto;
        }

        public async Task<IResponseDto<TokenDto>> GetTokenAsync()
        {
            var requestDto = new RequestDto() { DeviceId = device.DeviceId };

            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(ApiRouteLink.RequestRefreshToken.Absolute(device.ServerBaseAddress)),
                Version = new Version(ApiRouteLink.RequestRefreshToken.Version),
                Method = ApiRouteLink.RequestRefreshToken.Method,
                Content = RequestDto.BuildHttpContentAsJson(requestDto)
            };
        
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await device.GetTokenAsync());
            var response = await httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
                return new ResponseDto<TokenDto>(null) { StatuesCode = response.StatusCode };

            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDto>>();
            await device.SaveTokenAsync(responseDto?.Payload?.Token);
            responseDto.StatuesCode = response.StatusCode;
            return responseDto;
        }

        public Task<IResponseDto> LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IResponseDto> UpdateEmailAsync(UpdateEmailAsync loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<IResponseDto> UpdatePasswordAsync(UpdatePasswordDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<IRequestDto> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            throw new NotImplementedException();
        }
    }
}
