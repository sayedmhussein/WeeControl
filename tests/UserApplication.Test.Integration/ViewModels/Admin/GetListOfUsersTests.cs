using System;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Application.Essential;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.Services;
using WeeControl.User.UserApplication.Test.Integration.ViewModels.Authorization;
using WeeControl.User.UserApplication.ViewModels.Admin;
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
                var territory = TerritoryDbo.Create("TRR", "TRR", "TRR", "TRR");
                db.Territories.Add(territory);
                db.SaveChanges();
                var user = UserDbo.Create(
                    "email@email.com",
                    "username",
                    TestHelper<object>.GetEncryptedPassword("password"), "TRR");
                db.Users.Add(user);
                db.SaveChanges();

                var claim = ClaimDbo.Create(user.UserId, ClaimsTagsList.Claims.Administrator,
                    ClaimsTagsList.Tags.SuperUser, user.UserId);
                db.Claims.Add(claim);
                db.SaveChanges();
            });
        }).CreateClient();
        
        using var helper = new TestHelper<AdminViewModel>(httpClient);
        await helper.Authorize("username", "password");

        await helper.ViewModel.GetListOfUsers();

        Assert.Equal(2, helper.ViewModel.ListOfUsers.Count());
    }
    
    [Fact]
    public async void WhenSuccessNotFullList()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public async void WhenNotLoggedIn()
    {
        throw new NotImplementedException();
    }
    
    [Fact]
    public async void WhenNotAdminUser()
    {
        throw new NotImplementedException();
    }
}