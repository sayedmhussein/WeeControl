using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeeControl.Frontend.CommonLib.Interfaces;
using WeeControl.SharedKernel.DtosV1;
using WeeControl.SharedKernel.DtosV1.Employee;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.Routing;

namespace WeeControl.Frontend.CommonLib.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IDevice device;
        private readonly IApiRoute apiRoute;
        private readonly IHttpService httpService;
        private readonly IAuthenticationRefresh authenticationRefresh;
        private readonly ILocalStorage localStorage;

        private const string Token = "Token";
        private const string FullName = "Fullname";
        private const string PhotoUrl = "Photourl";

        public AuthenticationService(IDevice device, IApiRoute apiRoute, IHttpService httpService, IAuthenticationRefresh authenticationRefresh)
        {
            this.device = device;
            this.apiRoute = apiRoute;
            this.httpService = httpService;
            this.authenticationRefresh = authenticationRefresh;
            localStorage = device.LocalStorage;
        }
        
        public Task ResetPassword()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IResponseDto> Login(CreateLoginDto dto)
        {
            var requestDto = new RequestDto<CreateLoginDto>() { DeviceId = device.DeviceId, Payload = dto };
            
            HttpRequestMessage requestMessage = new();
            requestMessage.Method = HttpMethod.Post;
            requestMessage.RequestUri = new Uri(apiRoute.GetRoute(ApiRouteEnum.EmployeeSession), UriKind.Relative);
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

        public Task<IResponseDto> Refresh()
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