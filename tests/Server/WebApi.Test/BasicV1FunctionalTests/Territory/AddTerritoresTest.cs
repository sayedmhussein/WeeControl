//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http.Headers;
//using System.Net.Http.Json;
//using Microsoft.AspNetCore.Mvc.Testing;
//using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
//using WeeControl.SharedKernel.BasicSchemas.Territory.DtosV1;
//using Xunit;

//namespace WeeControl.Server.WebApi.Test.BasicV1FunctionalTests.Territory
//{
//    public class AddTerritoresTest : BaseFunctionalTest, IClassFixture<WebApplicationFactory<Startup>>
//    {
//        private readonly Uri RequstUri;
//        private readonly WebApplicationFactory<Startup> factory;

//        public AddTerritoresTest(WebApplicationFactory<Startup> factory)
//            : base(factory.CreateClient(), EmployeeName.Admin)
//        {
//            RequstUri = new Uri(BaseUri, ApiRoute[ApiRouteEnum.Territory]);
//            this.factory = factory;
//        }

//        [Fact]
//        public async void WhenAddingTerritoryWithoutToken_ReturnUnauthorizedHttpCodeRespose()
//        {
//            var client = factory.CreateClient();
//            var content = GetHttpContentAsJson(new TerritoryDto());

//            var response = await client.PostAsync(RequstUri, content);

//            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//        }

//        [Fact]
//        public async void WhenAddingBadTerritory_ReturnBadRequestHttpCodeRespose()
//        {
//            var client = factory.CreateClient();
//            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);
//            var content = GetHttpContentAsJson(new TerritoryDto());

//            var response = await client.PostAsync(RequstUri, content);

//            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//        }

//        [Fact]
//        public async void WhenAddingCorrectTerritoryWithToken_ReturnCreatedHttpCodeRespose()
//        {
//            var client = factory.CreateClient();
//            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);
//            var content = GetHttpContentAsJson(new TerritoryDto() { CountryId = "EGP", Name = new Random().NextDouble().ToString() });

//            var response = await client.PostAsync(RequstUri, content);

//            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
//        }

//        [Fact]
//        public async void WhenAddingCorrectTerritoryButRandomParent_ReturnConflictHttpCodeRespose()
//        {
//            var client = factory.CreateClient();
//            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);
//            var content = GetHttpContentAsJson(new TerritoryDto() { CountryId = "EGP", Name = new Random().NextDouble().ToString(), ReportToId = Guid.NewGuid() });

//            var response = await client.PostAsync(RequstUri, content);

//            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
//        }

//        [Fact]
//        public async void WhenAddingCorrectTerritoryAndCorrectParentWithToken_ReturnCreatedHttpCodeRespose()
//        {
//            var client = factory.CreateClient();
//            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Token);

//            var response1 = await client.GetAsync(RequstUri);
//            response1.EnsureSuccessStatusCode();
//            var list = await response1.Content.ReadFromJsonAsync<IEnumerable<TerritoryDto>>();

//            var content = GetHttpContentAsJson(new TerritoryDto() { CountryId = "EGP", Name = new Random().NextDouble().ToString(), ReportToId = list.FirstOrDefault().Id });

//            var response = await client.PostAsync(RequstUri, content);

//            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
//        }
//    }
//}
