using System;
using System.Net;
using System.Net.Http;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.SharedKernel.DtosV1;
using WeeControl.SharedKernel.DtosV1.Territory;
using WeeControl.SharedKernel.Routing;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.V1.Territory
{
    public class HttpPostTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly IFunctionalTest test;
        private readonly IFunctionalAuthorization authorization;
        private readonly Uri routeUri;

        public HttpPostTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            test = new FunctionalTest(factory, HttpMethod.Post, typeof(HttpPostTests).Namespace);
            authorization = new FunctionalAuthorization(test);
            routeUri = test.GetUri(ApiRouteEnum.Territory);
        }

        [Fact]
        public async void WhenWithoutToken_ReturnUnAuthorized()
        {
            var response = await test.GetResponseMessageAsync(routeUri);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenPosting_ReturnCreated()
        {
            var token = await authorization.GetTokenAsync("admin", "admin");
            var requestDto = new RequestDto<TerritoryDto>()
            {
                DeviceId = test.DeviceId,
                Payload = new TerritoryDto()
                {
                    Name = new Random().NextDouble().ToString(),
                    CountryId = "TST"
                }
            };
            var response = await test.GetResponseMessageAsync(routeUri, test.GetHttpContentAsJson(requestDto), token);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async void WhenPostingSameCountryAndNameTwice_ReturnConflict()
        {
            var token = await authorization.GetTokenAsync("admin", "admin");
            var dto = new RequestDto<TerritoryDto>()
            {
                DeviceId = test.DeviceId,
                Payload = new TerritoryDto()
                {
                    Name = new Random().NextDouble().ToString(),
                    CountryId = "TST"
                }
            };
            test.RequestMessage.Content = test.GetHttpContentAsJson(dto);
            test.RequestMessage.RequestUri = routeUri;
            var request1 = test.RequestMessage;
            var request2 = await test.CloneRequestMessageAsync(request1);

            var response = await test.GetResponseMessageAsync(request1, token);
            response.EnsureSuccessStatusCode();

            var response2 = await test.GetResponseMessageAsync(request2, token);

            Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);
        }

        [Fact]
        public async void WhenPostingInvalidContent_ReturnBadRequest()
        {
            var token = await authorization.GetTokenAsync("admin", "admin");

            var response = await test.GetResponseMessageAsync(routeUri, test.GetHttpContentAsJson(new TerritoryDto()), token);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
