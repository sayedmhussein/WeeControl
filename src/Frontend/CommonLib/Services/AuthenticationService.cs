using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
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
        private readonly IHttpClientFactory clientFactory;
        private readonly IDevice device;
        private readonly IApiRoute apiRoute;
        private readonly IHttpService httpService;
        private readonly ILocalStorage localStorage;

        private const string Token = "token";
        private const string FullName = "fullname";
        private const string PhotoUrl = "photourl";
        

        public IEnumerable<Claim> Claims { get; }

        public AuthenticationService(IHttpClientFactory clientFactory, IDevice device, IApiRoute apiRoute, IHttpService httpService)
        {
            this.clientFactory = clientFactory;
            this.device = device;
            this.apiRoute = apiRoute;
            this.httpService = httpService;
            localStorage = device.LocalStorage;
        }
        
        public Task Initialize()
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
            Console.Write("Response code is: ");
            Console.WriteLine(response.StatusCode);
            
            var client = clientFactory.CreateClient("NoAuth");
            
            //var response = await client.PostAsJsonAsync("/Api/Employee/Session", requestDto);

            IResponseDto responseDto = new ResponseDto()
            {
                HttpStatuesCode = (int)response.StatusCode
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

        public Task<IResponseDto> Logout()
        {
            localStorage.ClearItems();
            IResponseDto dto = new ResponseDto();
            return Task.FromResult(dto);
        }

        private async Task UpdateUserInfo(EmployeeTokenDto dto)
        {
            await localStorage.SetItem(Token, dto.Token);
            await localStorage.SetItem(FullName, dto.FullName);
            await localStorage.SetItem(PhotoUrl, dto.PhotoUrl);
        }
    }
}