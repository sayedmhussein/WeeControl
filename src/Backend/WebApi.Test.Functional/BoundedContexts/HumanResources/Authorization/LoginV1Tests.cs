using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel.BoundedContextDtos.HumanResources.Authorization;
using WeeControl.Common.SharedKernel.Obsolute.Common;
using WeeControl.Common.SharedKernel.Obsolute.Employee;
using WeeControl.Common.SharedKernel.Routing;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.BoundedContexts.HumanResources.Authorization
{
    public class LoginV1Tests : IClassFixture<CustomWebApplicationFactory<Startup>>, ITestScenarios
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly IFunctionalTest test;
        private readonly IFunctionalAuthorization authorization;
        private readonly Uri routeUri;
        
        public LoginV1Tests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            test = new FunctionalTest(factory, HttpMethod.Post, typeof(LoginV1Tests).Namespace);
            authorization = new FunctionalAuthorization(test);
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
            var content = test.GetHttpContentAsJson(new RequestDto<RequestNewTokenDto>()
            {
                DeviceId = test.DeviceId,
                Payload = new RequestNewTokenDto()
                {
                    Username = "admin",
                    Password = "admin"
                }
            });

            var response = await test.GetResponseMessageAsync(routeUri, content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var tokenDto = await response.Content.ReadFromJsonAsync<ResponseDto<EmployeeTokenDto>>();
            Assert.NotEmpty(tokenDto.Payload.Token);

            
        }
    }
}