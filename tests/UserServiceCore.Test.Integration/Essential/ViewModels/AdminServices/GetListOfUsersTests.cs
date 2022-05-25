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
// using WeeControl.User.UserServiceCore;
// using WeeControl.User.UserServiceCore.Enums;
// using WeeControl.WebApi;
// using Xunit;
//
// namespace WeeControl.User.ServiceCore.Test.Integration.Essential.Essentials.AdminServices;
//
// public class GetListOfUsersTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
// {
//     private readonly CustomWebApplicationFactory<Startup> factory;
//     private DeviceServiceMock deviceMock;
//
//     public GetListOfUsersTests(CustomWebApplicationFactory<Startup> factory)
//     {
//         this.factory = factory;
//         deviceMock = new DeviceServiceMock(nameof(GetListOfUsersTests));
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
//                     db.Users.AddRange(new List<UserDbo>()
//                     {
//                         UserDbo.Create("email1@email.com", "username1", "password"),
//                         UserDbo.Create("email2@email.com", "username2", "password"),
//                         UserDbo.Create("email3@email.com", "username3", "password")
//                     });
//                     db.SaveChanges();
//                 });
//             })
//             .CreateClient();
//
//         var token = await GetTokenTests.GetRefreshedTokenAsync(client, "username", "password",
//             nameof(GetListOfUsersTests));
//         deviceMock.StorageMock.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);
//
//         var services = new ServiceCollection();
//         services.AddScoped(_ => deviceMock.GetObject(client));
//         DependencyExtension.AddUserServiceCore(services);
//         var service = services.BuildServiceProvider().GetService<IAdminService>();
//
//         var list = await service.GetListOfUsers();
//         
//         Assert.Equal(4, list.Count());
//     }
// }