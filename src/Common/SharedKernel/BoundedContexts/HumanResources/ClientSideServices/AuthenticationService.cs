using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeeControl.Common.SharedKernel.BoundedContexts.HumanResources.Authentication;
using WeeControl.Common.SharedKernel.BoundedContexts.Shared;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Obsolutes.Dtos;

namespace WeeControl.Common.SharedKernel.BoundedContexts.HumanResources.ClientSideServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient client;
        private readonly IClientDevice clientDevice;
        
        public AuthenticationService(HttpClient client, IClientDevice device)
        {
            this.client = client;
            this.clientDevice = device;
        }
        
        public AuthenticationService(IHttpClientFactory clientFactory, IClientDevice device)
        {
            client = clientFactory.CreateClient("UnSecured");
            clientDevice = device ?? throw new ArgumentNullException("Client Device is Null!");
        }
        
        public async Task<IResponseDto> RequestNewToken(RequestNewTokenDto dto)
        {
            var requestDto = new RequestDto<RequestNewTokenDto>(clientDevice.DeviceId, dto);
            
            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(ApiRouteLink.HumanResources.Authorization.RequestNewToken.Absolute),
                Version = new Version(ApiRouteLink.HumanResources.Authorization.RequestNewToken.Version),
                Method = ApiRouteLink.HumanResources.Authorization.RequestNewToken.Method,
                Content = RequestDto.BuildHttpContentAsJson(requestDto)
            };

            var response = await client.SendAsync(message);

            if (response.IsSuccessStatusCode)
            {
                var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<EmployeeTokenDto>>();
                await clientDevice.SaveTokenAsync(responseDto?.Payload.Token);
                await clientDevice.SaveUserNameTask(responseDto?.Payload.FullName);
                await clientDevice.SaveUserPhotoUrlAsync(responseDto?.Payload.PhotoUrl);
                
                TokenChanged?.Invoke(this, responseDto?.Payload.Token);
            }

            return new ResponseDto(response.StatusCode);
        }

        public async Task<IResponseDto> RefreshCurrentToken()
        {
            HttpRequestMessage message = new()
            {
                RequestUri = new Uri(ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Absolute),
                Version = new Version(ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Version),
                Method = ApiRouteLink.HumanResources.Authorization.RequestRefreshToken.Method,
                Content = RequestDto.BuildHttpContentAsJson(new RequestDto(clientDevice.DeviceId))
            };
            
            await UpdateAuthorizationHeader();
            var response = await client.SendAsync(message);
            
            if (response.IsSuccessStatusCode)
            {
                var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<EmployeeTokenDto>>();
                await clientDevice.SaveTokenAsync(responseDto?.Payload.Token);
                await clientDevice.SaveUserNameTask(responseDto?.Payload.FullName);
                await clientDevice.SaveUserPhotoUrlAsync(responseDto?.Payload.PhotoUrl);
                
                TokenChanged?.Invoke(this, responseDto?.Payload.Token);
            }

            return new ResponseDto(response.StatusCode);
        }

        public async Task<IResponseDto> Logout()
        {
            IResponseDto responseDto = new ResponseDto();
            try
            {
                await UpdateAuthorizationHeader();
                
                HttpRequestMessage message = new()
                {
                    RequestUri = new Uri(ApiRouteLink.HumanResources.Authorization.Logout.Absolute),
                    Version = new Version(ApiRouteLink.HumanResources.Authorization.Logout.Version),
                    Method = ApiRouteLink.HumanResources.Authorization.Logout.Method,
                    Content = RequestDto.BuildHttpContentAsJson(new RequestDto(clientDevice.DeviceId))
                };
                
                var response = await client.SendAsync(message);
                responseDto.StatuesCode = response.StatusCode;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                responseDto.StatuesCode = HttpStatusCode.InternalServerError;
                throw;
            }
            
            await clientDevice.ClearUserDataAsync();
            TokenChanged?.Invoke(this, string.Empty);
            return responseDto;
        }

        public Task<IResponseDto> RequestPasswordReset(RequestPasswordResetDto dto)
        {
            throw new System.NotImplementedException();
        }

        public Task<IResponseDto> SetNewPassword(SetNewPasswordDto dto)
        {
            throw new System.NotImplementedException();
        }

        public event EventHandler<string> TokenChanged;

        private async Task UpdateAuthorizationHeader()
        {
            var token = await clientDevice.GetTokenAsync();
            
            if (string.IsNullOrWhiteSpace(token) == false)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                client.DefaultRequestHeaders.Authorization = null;
            }
            
        }
    }
}