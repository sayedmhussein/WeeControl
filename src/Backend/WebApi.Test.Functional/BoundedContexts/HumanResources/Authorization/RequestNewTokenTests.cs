using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.BoundedContextDtos.HumanResources.Authorization;
using WeeControl.Common.SharedKernel.Obsolute.Common;
using WeeControl.Common.SharedKernel.Obsolute.Employee;
using WeeControl.Common.SharedKernel.Routing;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.BoundedContexts.HumanResources.Authorization
{
    public class RequestNewTokenTests : IClassFixture<CustomWebApplicationFactory<Startup>>, ITestsNotRequireAuthentication
    {
        #region static
        private const string Route = ApiRouteLink.HumanResources.Authorization.RequestNewToken.Absolute;
        private static readonly HttpMethod Method = ApiRouteLink.HumanResources.Authorization.RequestNewToken.Method;
        private const string Version = ApiRouteLink.HumanResources.Authorization.RequestNewToken.Version;
        
        public static async Task<string> GetNewTokenAsync(CustomWebApplicationFactory<Startup> factory, string device)
        {
            var test = new FunctionalTest(factory, device, Method, Version);
            
            var content = test.GetHttpContentAsJson(new RequestDto<RequestNewTokenDto>()
            {
                DeviceId = test.DeviceId,
                Payload = new RequestNewTokenDto()
                {
                    Username = "admin",
                    Password = "admin"
                }
            });

            var response = await test.GetResponseMessageAsync(new Uri(Route), content);
            response.EnsureSuccessStatusCode();
            var tokenDto = await response.Content.ReadFromJsonAsync<ResponseDto<EmployeeTokenDto>>();

            return tokenDto?.Payload.Token;
        }
        #endregion
        
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly IFunctionalTest test;
        private readonly Uri routeUri;
        
        public RequestNewTokenTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            test = new FunctionalTest(factory, typeof(RequestNewTokenTests).Namespace, Method, Version);
            routeUri = test.GetUri(ApiRouteEnum.EmployeeSession);
        }

        [Fact]
        public async void WhenSendingInvalidRequest_HttpResponseIsBadRequest()
        {
            var content = test.GetHttpContentAsJson(new RequestDto<string>() { Payload = "" });

            var response = await test.GetResponseMessageAsync(routeUri, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
        {
            var token = await GetNewTokenAsync(factory, typeof(RequestNewTokenTests).Namespace);
            
            Assert.NotEmpty(token);
        }
    }
}