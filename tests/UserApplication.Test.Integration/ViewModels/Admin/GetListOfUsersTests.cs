using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Application.Interfaces;
using WeeControl.Domain.Contexts.Essential;
using WeeControl.SharedKernel;
using WeeControl.User.UserApplication.ViewModels.Essential;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.ViewModels.Admin;

public class GetListOfUsersTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public GetListOfUsersTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async void WhenSuccess()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var territory = TerritoryDbo.Create("TRR", null, "TRR", "TRR");
                db.Territories.Add(territory);
                db.SaveChanges();
                var user = TestHelper<object>.GetUserDboWithEncryptedPassword("username", "password", "TRR");
                db.Users.Add(user);
                db.Users.Add(TestHelper<object>.GetUserDboWithEncryptedPassword("another", "another", "TRR"));
                db.SaveChanges();

                var claim = ClaimDbo.Create(user.UserId, ClaimsValues.ClaimTypes.Administrator,
                    ClaimsValues.ClaimValues.SuperUser, user.UserId);
                db.Claims.Add(claim);
                db.SaveChanges();
            });
        }).CreateClient();
        
        using var helper = new TestHelper<ListOfUsersViewModel>(httpClient);
        await helper.Authorize("username", "password");

        await helper.ViewModel.GetListOfUsers();

        Assert.Equal(2, helper.ViewModel.ListOfUsers.Count());
    }
    
    [Fact]
    public async void WhenSuccessNotFullList()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var territory1 = TerritoryDbo.Create("TRR1", null, "TRR", "TRR");
                var territory2 = TerritoryDbo.Create("TRR2", "TRR1", "TRR", "TRR");
                db.Territories.Add(territory1);
                db.Territories.Add(territory2);
                db.SaveChanges();
                var user = TestHelper<object>.GetUserDboWithEncryptedPassword("username", "password", "TRR");
                db.Users.Add(user);
                db.Users.Add(TestHelper<object>.GetUserDboWithEncryptedPassword("another", "password", "TRR1"));
                db.SaveChanges();

                var claim = ClaimDbo.Create(user.UserId, ClaimsValues.ClaimTypes.Administrator,
                    ClaimsValues.ClaimValues.SuperUser, user.UserId);
                db.Claims.Add(claim);
                db.SaveChanges();
            });
        }).CreateClient();
        
        using var helper = new TestHelper<ListOfUsersViewModel>(httpClient);
        await helper.Authorize("username", "password");

        await helper.ViewModel.GetListOfUsers();

        Assert.Single(helper.ViewModel.ListOfUsers);
    }
    
    [Fact]
    public async void WhenNotLoggedIn()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var territory = TerritoryDbo.Create("TRR", "TRR", "TRR", "TRR");
                db.Territories.Add(territory);
                db.SaveChanges();
                var user = TestHelper<object>.GetUserDboWithEncryptedPassword("username", "password", "TRR");
                db.Users.Add(user);
                db.SaveChanges();

                var claim = ClaimDbo.Create(user.UserId, ClaimsValues.ClaimTypes.Administrator,
                    ClaimsValues.ClaimValues.SuperUser, user.UserId);
                db.Claims.Add(claim);
                db.SaveChanges();
            });
        }).CreateClient();
        
        using var helper = new TestHelper<ListOfUsersViewModel>(httpClient);

        await helper.ViewModel.GetListOfUsers();

        Assert.Empty(helper.ViewModel.ListOfUsers);
    }
    
    [Fact]
    public async void WhenNotAdminUser()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var territory = TerritoryDbo.Create("TRR", "TRR", "TRR", "TRR");
                db.Territories.Add(territory);
                db.SaveChanges();
                var user = TestHelper<object>.GetUserDboWithEncryptedPassword("username", "password", "TRR");
                db.Users.Add(user);
                db.SaveChanges();

                var claim = ClaimDbo.Create(user.UserId, ClaimsValues.ClaimTypes.Field,
                    ClaimsValues.ClaimValues.SuperUser, user.UserId);
                db.Claims.Add(claim);
                db.SaveChanges();
            });
        }).CreateClient();
        
        using var helper = new TestHelper<ListOfUsersViewModel>(httpClient);
        await helper.Authorize("username", "password");

        await helper.ViewModel.GetListOfUsers();

        Assert.Empty(helper.ViewModel.ListOfUsers);
    }
}