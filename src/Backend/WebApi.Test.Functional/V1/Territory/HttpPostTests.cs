using System;
using System.Net;
using System.Net.Http;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.SharedKernel.DtosV1;
using WeeControl.SharedKernel.DtosV1.Territory;
using WeeControl.SharedKernel.Obsolutes;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.V1.Territory
{
    public class HttpPostTests :
        BaseFunctionalTest,
        IClassFixture<CustomWebApplicationFactory<Startup>>,
        IDisposable
    {
        private readonly Uri requestUri;

        public HttpPostTests(CustomWebApplicationFactory<Startup> factory) :
            base(factory, HttpMethod.Post, typeof(HttpGetTests).Namespace)
        {
            requestUri = GetUri(ApiRouteEnum.Territory);
        }

        [Fact]
        public async void WhenWithoutToken_ReturnUnAuthorized()
        {
            var response = await GetResponseMessageAsync(requestUri);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void WhenPosting_ReturnCreated()
        {
            var token = await authorization.GetTokenAsync("admin", "admin");
            var requestDto = new RequestDto<TerritoryDto>()
            {
                DeviceId = DeviceId,
                Payload = new TerritoryDto()
                {
                    Name = new Random().NextDouble().ToString(),
                    CountryId = "TST"
                }
            };
            var response = await GetResponseMessageAsync(requestUri, GetHttpContentAsJson(requestDto), token);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async void WhenPostingSameCountryAndNameTwice_ReturnConflict()
        {
            var token = await authorization.GetTokenAsync("admin", "admin");
            var dto = new RequestDto<TerritoryDto>()
            {
                DeviceId = DeviceId,
                Payload = new TerritoryDto()
                {
                    Name = new Random().NextDouble().ToString(),
                    CountryId = "TST"
                }
            };
            RequestMessage.Content = GetHttpContentAsJson(dto);
            RequestMessage.RequestUri = requestUri;
            var request1 = RequestMessage;
            var request2 = await CloneRequestMessageAsync(request1);

            var response = await GetResponseMessageAsync(request1, token);
            response.EnsureSuccessStatusCode();

            var response2 = await GetResponseMessageAsync(request2, token);

            Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);
        }

        [Fact]
        public async void WhenPostingInvalidContent_ReturnBadRequest()
        {
            var token = await authorization.GetTokenAsync("admin", "admin");

            var response = await GetResponseMessageAsync(requestUri, GetHttpContentAsJson(new TerritoryDto()), token);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
