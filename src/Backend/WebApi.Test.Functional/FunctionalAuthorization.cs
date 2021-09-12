using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeeControl.SharedKernel.EntityGroup.Employee.DtosV1;
using WeeControl.SharedKernel.Common;
using WeeControl.SharedKernel.DtosV1;

namespace WeeControl.Backend.WebApi.Test.Functional
{
    public class FunctionalAuthorization : IFunctionalAuthorization
    {
        private readonly IFunctionalTest httpMessage;

        public FunctionalAuthorization(IFunctionalTest httpMessage)
        {
            this.httpMessage = httpMessage;
        }

        public async Task<string> GetTokenAsync(string username, string password, string device = null)
        {
            var loginToken = await CreateTokenAsync(new RequestDto<CreateLoginDto>()
            {
                DeviceId = device ?? httpMessage.DeviceId,
                Payload = new CreateLoginDto()
                {
                    Username = username,
                    Password = password,
                }
            });

            var refreshToken = await RefreshTokenAsync(
                new RequestDto<RefreshLoginDto>()
                {
                    DeviceId = httpMessage.DeviceId,
                }
                , loginToken);

            return refreshToken;
        }

        private async Task<string> CreateTokenAsync(RequestDto<CreateLoginDto> dto)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Version = new Version("1.0"),
                RequestUri = httpMessage.GetUri(ApiRouteEnum.EmployeeSession),
                Content = httpMessage.GetHttpContentAsJson(dto)
            };

            var response = await httpMessage.GetResponseMessageAsync(request);
            response.EnsureSuccessStatusCode();
            var tokenDto = await response.Content.ReadFromJsonAsync<ResponseDto<EmployeeTokenDto>>();

            return tokenDto.Payload.Token;
        }

        private async Task<string> RefreshTokenAsync(RequestDto<RefreshLoginDto> dto, string token)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                Version = new Version("1.0"),
                RequestUri = httpMessage.GetUri(ApiRouteEnum.EmployeeSession),
                Content = httpMessage.GetHttpContentAsJson(dto)
            };

            //
            var response2 = await httpMessage.GetResponseMessageAsync(request, token);
            response2.EnsureSuccessStatusCode();
            var tokenDto2 = await response2.Content.ReadFromJsonAsync<ResponseDto<EmployeeTokenDto>>();
            var token2 = tokenDto2.Payload.Token;
            //
            return token2;
        }
    }
}
