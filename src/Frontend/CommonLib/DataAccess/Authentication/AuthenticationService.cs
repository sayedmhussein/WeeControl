using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.BoundedContextDtos.HumanResources.Authorization;
using WeeControl.Common.SharedKernel.BoundedContextDtos.Shared;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Obsolutes.Dtos;
using WeeControl.Frontend.CommonLib.Interfaces;

namespace WeeControl.Frontend.CommonLib.DataAccess.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IDevice device;
        private readonly IHttpService httpService;
        private readonly IAuthenticationRefresh authenticationRefresh;
        private readonly ILocalStorage localStorage;

        private const string Token = "Token";
        private const string FullName = "Fullname";
        private const string PhotoUrl = "Photourl";

        public AuthenticationService(IDevice device, IHttpService httpService, IAuthenticationRefresh authenticationRefresh)
        {
            this.device = device;
            this.httpService = httpService;
            this.authenticationRefresh = authenticationRefresh;
            localStorage = device.LocalStorage;
        }
        
        public Task<IResponseDto> RequestPasswordReset(RequestPasswordResetDto dto)
        {
            throw new System.NotImplementedException();
        }

        public Task<IResponseDto> SetNewPassword(SetNewPasswordDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponseDto> RequestNewToken(RequestNewTokenDto dto)
        {
            var requestDto = new RequestDto<RequestNewTokenDto>(device.DeviceId, dto);
            
            HttpRequestMessage requestMessage = new();
            requestMessage.Method = HttpMethod.Post;
            requestMessage.RequestUri = new Uri(ApiRouteLink.HumanResources.Authorization.RequestNewToken.Relative, UriKind.Relative);
            requestMessage.Content = httpService.GetHttpContentAsJson(requestDto);
            
            var response = await httpService.SendAsync(requestMessage);

            IResponseDto responseDto = new ResponseDto()
            {
                HttpStatuesCode = response.StatusCode
            };
            
            if (response.IsSuccessStatusCode)
            {
                var responseDto_ = await response.Content.ReadFromJsonAsync<ResponseDto<EmployeeTokenDto>>();
                await UpdateUserInfo(responseDto_.Payload);
            }

            return responseDto;
        }

        public Task<IResponseDto> RefreshCurrentToken()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IResponseDto> Logout()
        {
            await localStorage.ClearItems();
            
            await authenticationRefresh.AuthenticationRefreshedAsync();
            
            IResponseDto dto = new ResponseDto();
            return dto;
        }

        private async Task UpdateUserInfo(EmployeeTokenDto dto)
        {
            await localStorage.SetItem(Token, dto.Token);
            await localStorage.SetItem(FullName, dto.FullName);
            await localStorage.SetItem(PhotoUrl, dto.PhotoUrl);
            
            await authenticationRefresh.AuthenticationRefreshedAsync();
        }
    }
}