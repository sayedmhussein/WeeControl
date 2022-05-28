using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.EssentialContext;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.DataTransferObjects.Authentication;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.SharedKernel.Services;
using WeeControl.User.UserApplication.ViewModels.Authentication;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.ViewModels.Authorization;

public class LogoutTests : IClassFixture<CustomWebApplicationFactory<Startup>>, System.IDisposable
{
    #region Preparation
    private LogoutViewModel vm;
    private readonly HttpClient httpClient;
    private DeviceServiceMock deviceMock;

    private readonly UserDbo normalUserDbo = 
        UserDbo.Create("normal@test.test", "normal", new PasswordSecurity().Hash("normal"),"TST");
    private readonly UserDbo lockedUserDbo = 
        UserDbo.Create("locked@test.test", "locked", new PasswordSecurity().Hash("locked"),"TST");

    public LogoutTests(CustomWebApplicationFactory<Startup> factory)
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
            });
        }).CreateClient();
        
        
        deviceMock = new DeviceServiceMock(nameof(LogoutTests));
        
        var appServiceCollection = new ServiceCollection();
        appServiceCollection.AddViewModels();
        appServiceCollection.AddScoped(p => deviceMock.GetObject(httpClient));
        
        using var scope = appServiceCollection.BuildServiceProvider().CreateScope();
        vm = scope.ServiceProvider.GetRequiredService<LogoutViewModel>();
    }
    
    public void Dispose()
    {
        deviceMock = null;
        vm = null;
    }
    #endregion

    #region Success
    [Fact]
    public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
    {
        var token = await LoginTests.GetNewToken(httpClient, normalUserDbo.Username, "normal", nameof(LogoutTests));
        deviceMock.InjectTokenToMock(token);
        //deviceMock.SecurityMock.Setup(x => x.GetTokenAsync()).ReturnsAsync(token);

        await vm.LogoutAsync();
            
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Authentication.Login, It.IsAny<bool>()), Times.Once);
        
        deviceMock.StorageMock.Verify(x => 
            x.ClearAsync(), Times.AtLeastOnce);
    }
    

    #endregion
    

    
}