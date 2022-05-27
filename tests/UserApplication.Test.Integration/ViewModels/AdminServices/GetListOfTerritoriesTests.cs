// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Microsoft.Extensions.DependencyInjection;
// using Moq;
// using WeeControl.Application.EssentialContext;
// using WeeControl.Domain.Essential.Entities;
// using WeeControl.SharedKernel.Essential;
// using WeeControl.SharedKernel.Services;
// using WeeControl.User.ServiceCore.Test.Integration.Essential.Essentials.UserServices;
// using WeeControl.User.UserApplication;
// using WeeControl.User.UserApplication.Enums;
// using WeeControl.WebApi;
// using Xunit;
//
// namespace WeeControl.User.ServiceCore.Test.Integration.Essential.Essentials.AdminServices;
//
// public class GetListOfTerritoriesTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
// {
//     private readonly CustomWebApplicationFactory<Startup> factory;
//     private DeviceServiceMock deviceMock;
//
//     public GetListOfTerritoriesTests(CustomWebApplicationFactory<Startup> factory)
//     {
//         this.factory = factory;
//         deviceMock = new DeviceServiceMock(nameof(GetListOfTerritoriesTests));
//     }
//     
//     public void Dispose()
//     {
//         deviceMock = null;
//     }
//
//     [Fact]
//     public async void WhenTerritoryExistAndUserAuthorized_ReturnAll()
//     {
//         var client = factory.WithWebHostBuilder(builder =>
//             {
//                 builder.ConfigureServices(services =>
//                 {
//                     using var scope = services.BuildServiceProvider().CreateScope();
//                     var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
//                     db.Users.Add(UserDbo.Create("email@email.com", "username", new PasswordSecurity().Hash("password")));
//                     db.Territories.AddRange(new List<TerritoryDbo>()
//                     {
//                         TerritoryDbo.Create("ter1", null, "CNT", "Name1"),
//                         TerritoryDbo.Create("ter2", null, "CNT", "Name1"),
//                         TerritoryDbo.Create("ter3", null, "CNT", "Name1")
//                     });
//                     db.SaveChanges();
//                 });
//             })
//             .CreateClient();
//
//         var token = await GetTokenTests.GetRefreshedTokenAsync(client, "username", "password",
//             nameof(GetListOfTerritoriesTests));
//         deviceMock.StorageMock.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);
//
//         var services = new ServiceCollection();
//         services.AddScoped(_ => deviceMock.GetObject(client));
//         DependencyExtension.AddUserServiceCore(services);
//         var service = services.BuildServiceProvider().GetService<IAdminService>();
//
//         var list = await service.GetListOfTerritories();
//         
//         Assert.Equal(3, list.Count());
//     }
// }