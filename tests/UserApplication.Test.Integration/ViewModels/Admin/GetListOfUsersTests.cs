using System;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Application.EssentialContext;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.Services;
using WeeControl.User.UserApplication.Test.Integration.ViewModels.Authorization;
using WeeControl.User.UserApplication.ViewModels.Admin;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.ViewModels.Admin;

public class GetListOfUsersTests : IClassFixture<CustomWebApplicationFactory<Startup>>, System.IDisposable
{
    private AdminViewModel vm;
    private readonly HttpClient httpClient;
    private DeviceServiceMock deviceMock;

    private readonly UserDbo normalUserDbo = 
        UserDbo.Create("normal@test.test", "normal", new PasswordSecurity().Hash("normal"),"TST");
    private readonly UserDbo lockedUserDbo = 
        UserDbo.Create("locked@test.test", "locked", new PasswordSecurity().Hash("locked"),"TST");

    public GetListOfUsersTests(CustomWebApplicationFactory<Startup> factory)
    {
        httpClient = factory.WithWebHostBuilder(builder =>
        {
            lockedUserDbo.Suspend("For Test Only");
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                db.Users.Add(normalUserDbo);
                db.Users.Add(lockedUserDbo);
                db.SaveChanges();

                db.Territories.Add(TerritoryDbo.Create("TST", "", "TST", "TST"));
                db.SaveChanges();
            });
        }).CreateClient();
        
        deviceMock = new DeviceServiceMock(nameof(GetListOfUsersTests));
        
        var appServiceCollection = new ServiceCollection();
        appServiceCollection.AddViewModels();
        appServiceCollection.AddScoped(p => deviceMock.GetObject(httpClient));
        
        using var scope = appServiceCollection.BuildServiceProvider().CreateScope();
        vm = scope.ServiceProvider.GetRequiredService<AdminViewModel>();
    }
    
    public void Dispose()
    {
        deviceMock = null;
        vm = null;
    }
    
    [Fact]
    public async void WhenSuccess()
    {
        var token = await LoginTests.GetNewToken(httpClient, normalUserDbo.Username, "normal", nameof(LogoutTests));
        deviceMock.InjectTokenToMock(token);

        await vm.GetListOfUsers();

        Assert.Equal(2, vm.ListOfUsers.Count());
    }
    
    [Fact]
    public async void WhenSuccessNotFullList()
    {
        var token = await LoginTests.GetNewToken(httpClient, normalUserDbo.Username, "normal", nameof(LogoutTests));
        deviceMock.InjectTokenToMock(token);

        throw new NotImplementedException();
    }
    
    [Fact]
    public async void WhenNotLoggedIn()
    {
        var token = await LoginTests.GetNewToken(httpClient, normalUserDbo.Username, "normal", nameof(LogoutTests));
        deviceMock.InjectTokenToMock(token);

        throw new NotImplementedException();
    }
    
    [Fact]
    public async void WhenNotAdminUser()
    {
        var token = await LoginTests.GetNewToken(httpClient, normalUserDbo.Username, "normal", nameof(LogoutTests));
        deviceMock.InjectTokenToMock(token);

        throw new NotImplementedException();
    }
}