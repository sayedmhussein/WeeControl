// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using Microsoft.Extensions.DependencyInjection;
// using Newtonsoft.Json;
// using WeeControl.Backend.Persistence.BoundedContexts.HumanResources;
// using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
// using WeeControl.Common.SharedKernel.DataTransferObjectV1.Common;
// using WeeControl.Common.SharedKernel.DataTransferObjectV1.Territory;
// using WeeControl.Common.SharedKernel.Routing;
// using Xunit;
//
// namespace WeeControl.Backend.WebApi.Test.Functional.V1.Territory
// {
//      
//     public class HttpGetTests : IClassFixture<CustomWebApplicationFactory<Startup>>
//     {
//         private readonly CustomWebApplicationFactory<Startup> factory;
//         private readonly IFunctionalTest test;
//         private readonly IFunctionalAuthorization authorization;
//         private readonly Uri routeUri;
//         
//         public HttpGetTests(CustomWebApplicationFactory<Startup> factory)
//         {
//             this.factory = factory;
//             test = new FunctionalTest(factory, HttpMethod.Get, typeof(HttpGetTests).Namespace);
//             authorization = new FunctionalAuthorization(test);
//             routeUri = test.GetUri(ApiRouteEnum.Territory);
//         }
//
//         [Fact]
//         public async void WhenWithoutToken_ReturnUnauthorized()
//         {
//             var response = await test.GetResponseMessageAsync(routeUri);
//
//             Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//         }
//
//         [Fact]
//         public async void WhenWithToken_ReturnOkWithListOfTypeTerritory()
//         {
//             using var serviceScope = factory.Services.CreateScope();
//             var context = serviceScope.ServiceProvider.GetService<HumanResourcesDbContext>();
//             await context.Territories.AddAsync(new Domain.BoundedContexts.HumanResources.TerritoryModule.Entities.Territory() { CountryCode = "001", TerritoryName = "001" });
//             await context.Territories.AddAsync(new Domain.BoundedContexts.HumanResources.TerritoryModule.Entities.Territory() { CountryCode = "002", TerritoryName = "002" });
//             await context.SaveChangesAsync();
//
//             var token = await authorization.GetTokenAsync("admin", "admin");
//
//             var response = await test.GetResponseMessageAsync(test.GetUri(ApiRouteEnum.Territory), null, token);
//
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//
//             var response_str = await response.Content.ReadAsStringAsync();
//             var response_dto = JsonConvert.DeserializeObject<ResponseDto<IEnumerable<IdentifiedTerritoryDto>>>(response_str);
//
//             var territories = response_dto.Payload;
//             Assert.NotEmpty(territories);
//         }
//
//         [Fact]
//         public async void WhenQueringSpecificTerritory_ReturnCorrectCount()
//         {
//             using var serviceScope = factory.Services.CreateScope();
//             var context = serviceScope.ServiceProvider.GetService<HumanResourcesDbContext>();
//             var parent = context.Territories.FirstOrDefault();
//             await context.Territories.AddAsync(new Domain.BoundedContexts.HumanResources.TerritoryModule.Entities.Territory() { CountryCode = "001", TerritoryName = "001", ReportToId = parent.TerritoryCode });
//             await context.Territories.AddAsync(new Domain.BoundedContexts.HumanResources.TerritoryModule.Entities.Territory() { CountryCode = "002", TerritoryName = "002" });
//             await context.SaveChangesAsync();
//
//             var token = await authorization.GetTokenAsync("admin", "admin");
//             var uri = new UriBuilder(test.GetUri(ApiRouteEnum.Territory));
//             uri.Query = parent.TerritoryCode.ToString();
//             var response = await test.GetResponseMessageAsync(uri.Uri, null, token);
//
//             Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//
//             var response_str = await response.Content.ReadAsStringAsync();
//             var response_dto = JsonConvert.DeserializeObject<ResponseDto<IEnumerable<IdentifiedTerritoryDto>>>(response_str);
//
//             var territories = response_dto.Payload;
//             Assert.NotEmpty(territories);
//         }
//     }
// }
