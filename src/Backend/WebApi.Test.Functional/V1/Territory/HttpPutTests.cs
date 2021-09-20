using System;
using System.Net;
using System.Net.Http;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel.DtosV1;
using WeeControl.Common.SharedKernel.DtosV1.Common;
using WeeControl.Common.SharedKernel.DtosV1.Territory;
using WeeControl.Common.SharedKernel.Routing;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.V1.Territory
{
    public class HttpPutTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly IFunctionalTest test;
        private readonly IFunctionalAuthorization authorization;
        private readonly Uri routeUri;
        
        public HttpPutTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            test = new FunctionalTest(factory, HttpMethod.Put, typeof(HttpPutTests).Namespace);
            authorization = new FunctionalAuthorization(test);
            routeUri = test.GetUri(ApiRouteEnum.Territory);
        }

        [Fact]
        public async void WhenWithoutToken_ReturnUnAuthorized()
        {
            var uri = new Uri(routeUri, Guid.Empty.ToString());

            var response = await test.GetResponseMessageAsync(uri, test.GetHttpContentAsJson(new TerritoryDto()));

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenRandomTerritory_ReturnNotFound()
        {
            var uri = new Uri(routeUri, Guid.Empty.ToString());

            var dto = new RequestDto<TerritoryDto>()
            {
                DeviceId = test.DeviceId,
                Payload = new TerritoryDto()
                {
                    CountryId = "PUT",
                    Name = new Random().NextDouble().ToString()
                }
            };

            var token = await authorization.GetTokenAsync("admin", "admin");

            var response = await test.GetResponseMessageAsync(uri, test.GetHttpContentAsJson(dto), token);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
