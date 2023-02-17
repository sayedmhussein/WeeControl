using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Core.SharedKernel.Interfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.Test.ApiService.Internals.Interfaces;

public class DeviceSecurityTests
{
    [Theory]
    [InlineData("Token", true)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public async void IsAuthenticatedTests(string token, bool isTrue)
    {
        using var service = new TestingServiceHelper();
        service.StorageMock.Setup(x => x.GetKeyValue(IDeviceSecurity.TokenKeyName)).ReturnsAsync(token);
        
        Assert.Equal(isTrue, await service.GetService<IDeviceSecurity>().IsAuthenticated());
    }

    [Fact]
    public async void TokenSettingAndGetting()
    {
        using var testingService = new TestingServiceHelper();
        testingService.StorageMock.Setup(x => x.SaveKeyValue(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string k, string v) => testingService.StorageMock.Setup(x => x.GetKeyValue(k)).ReturnsAsync(v));

        const string token = "This should contains Token String";
        var service = testingService.GetService<IDeviceSecurity>();
        await service.UpdateToken(token);

        Assert.Equal(token, await service.GetToken());
    }

    [Fact]
    public async void DeletingTokenTest()
    {
        using var testService = new TestingServiceHelper();
        testService.StorageMock.Setup(x => x.SaveKeyValue(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string k, string v) => testService.StorageMock.Setup(x => x.GetKeyValue(k)).ReturnsAsync(v));
        const string token = "This should contains Token String";

        var service = testService.GetService<IDeviceSecurity>();
        await service.UpdateToken(token);
        await service.DeleteToken();

        Assert.NotEqual(token, await service.GetToken());
    }

    [Fact]
    public async void GettingClaimPrincipleTest()
    {
        using var testingService = new TestingServiceHelper();
        testingService.StorageMock.Setup(x => x.SaveKeyValue(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string k, string v) => testingService.StorageMock.Setup(x => x.GetKeyValue(k)).ReturnsAsync(v));
        var ci = new ClaimsIdentity("xxx");
        ci.AddClaim(new Claim("bla1", "bla2"));
        var key = Encoding.ASCII.GetBytes(new string('x', 30));
        var token = testingService.GetService<IJwtService>().GenerateToken(new SecurityTokenDescriptor()
        {
            Subject = ci,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        var service = testingService.GetService<IDeviceSecurity>();
        
        await service.UpdateToken(token);
        var cp = await service.GetClaimsPrincipal();

        Assert.Equal("bla1", cp.Claims.First().Type);
        Assert.Equal("bla2", cp.Claims.First().Value);
    }
}