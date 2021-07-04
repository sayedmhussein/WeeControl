//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http.Headers;
//using System.Net.Http.Json;
//using System.Web;
//using Microsoft.AspNetCore.Mvc.Testing;
//using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
//using WeeControl.SharedKernel.BasicSchemas.Territory.DtosV1;
//using Xunit;

//namespace WeeControl.Server.WebApi.Test.BasicV1FunctionalTests.Territory
//{
//    public class GetTerritoriesTests : BaseFunctionalTest, IClassFixture<WebApplicationFactory<Startup>>
//    {
//        private readonly Uri RequstUri;
//        private readonly WebApplicationFactory<Startup> factory;

//        public GetTerritoriesTests(WebApplicationFactory<Startup> factory)
//            : base(factory.CreateClient(), EmployeeName.Admin)
//        {
//            RequstUri = new Uri(BaseUri, ApiRoute[ApiRouteEnum.Territory]);
//            this.factory = factory;
//        }

//        [Fact]
//        public async void WhenGettingAllTerritoriesWithoutToken_ResponseIsUnauthorized()
//        {
//            var client = factory.CreateClient();

//            var response = await client.GetAsync(RequstUri);

//            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//        }

//        [Fact]
//        public async void WhenGettingAllTerritoriesWithToken_ReturnListOfTerritores()
//        {
//            var client = factory.CreateClient();
//            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);

//            var response = await client.GetAsync(RequstUri);

//            response.EnsureSuccessStatusCode();

//            var list = await response.Content.ReadFromJsonAsync<IEnumerable<TerritoryDto>>();
//            Assert.NotEmpty(list);
//        }

//        [Fact]
//        public async void WhenGettingAllTerritoriesWithTokenAndWithKnownTerritoryIdQuery_ReturnListOfTerritores()
//        {
//            var client = factory.CreateClient();
//            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);

//            var response1 = await client.GetAsync(RequstUri);
//            response1.EnsureSuccessStatusCode();
//            var list1 = await response1.Content.ReadFromJsonAsync<IEnumerable<TerritoryDto>>();

//            var builder = new UriBuilder(RequstUri);
//            var query = HttpUtility.ParseQueryString(string.Empty);
//            query["territoryid"] = list1.FirstOrDefault().Id.ToString();
//            builder.Query = query.ToString();

//            var response2 = await client.GetAsync(builder.ToString());
//            response2.EnsureSuccessStatusCode();
//            var list2 = await response2.Content.ReadFromJsonAsync<IEnumerable<TerritoryDto>>();

//            Assert.NotEmpty(list2);
//        }

//        [Fact]
//        public async void WhenGettingAllTerritoriesWithTokenAndWithRandomTerritoryIdQuery_ReturnNotFoundHttpStatuesCode()
//        {
//            var client = factory.CreateClient();
//            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);

//            var builder = new UriBuilder(RequstUri);
//            var query = HttpUtility.ParseQueryString(string.Empty);
//            query["territoryid"] = Guid.NewGuid().ToString();
//            builder.Query = query.ToString();

//            var response = await client.GetAsync(builder.ToString());

//            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
//        }
//    }
//}
