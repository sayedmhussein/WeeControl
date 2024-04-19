using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Core.SharedKernel;
using WeeControl.Core.SharedKernel.Interfaces;
using WeeControl.Host.WebApiService.Data;
using WeeControl.Host.WebApiService.Interfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.Test.ApiService.Internals.Services;

public class DeviceSecurityTests
{
    [Theory]
    [InlineData("Token", true)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public async void IsAuthenticatedTests(string token, bool isTrue)
    {
        using var service = new HostTestHelper(HttpStatusCode.OK);
        service.StorageMock.Setup(x => x.GetKeyValue(IDeviceSecurity.TokenKeyName)).ReturnsAsync(token);

        Assert.Equal(isTrue, await service.GetService<IDeviceSecurity>().IsAuthenticated());
    }

    [Fact]
    public async void TokenSettingAndGetting()
    {
        using var testingService = new HostTestHelper(HttpStatusCode.OK);
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
        using var testService = new HostTestHelper(HttpStatusCode.OK);
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
        using var testingService = new HostTestHelper(HttpStatusCode.OK);
        testingService.StorageMock.Setup(x => x.SaveKeyValue(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string k, string v) => testingService.StorageMock.Setup(x => x.GetKeyValue(k)).ReturnsAsync(v));


        var service = testingService.GetService<IDeviceSecurity>();

        await service.UpdateToken(GenerateToken(testingService));
        var cp = await service.GetClaimsPrincipal();

        Assert.Equal("bla1", cp.Claims.First().Type);
        Assert.Equal("bla2", cp.Claims.First().Value);
    }

    [Theory]
    [InlineData("SomeOtherPage", "Bla", false)]
    [InlineData(ApplicationPages.Essential.LoginPage, "", false)]
    [InlineData(ApplicationPages.Elevator.AdminPage, ClaimsValues.ClaimValues.Auditor, false)]
    [InlineData(ApplicationPages.Elevator.FieldPage, "", true)]
    [InlineData(ApplicationPages.Elevator.FieldPage, ClaimsValues.ClaimValues.Auditor, true)]
    public async void TestPageExistInClaims(string pageName, string authority, bool exist)
    {
        using var helper = new HostTestHelper(HttpStatusCode.OK);
        var service = helper.GetService<IDeviceSecurity>();
        await service.UpdateToken(GenerateToken(helper, new List<Claim>
        {
            new(ClaimsValues.ClaimTypes.Field, ClaimsValues.ClaimValues.Auditor)
        }));

        var testingService = helper.GetService<ISecurity>();
        var result = await testingService.PageExistInClaims(pageName, authority);

        Assert.Equal(exist, result);
    }

    [Fact]
    public async void TestGetAllowedPages()
    {
        using var helper = new HostTestHelper(HttpStatusCode.OK);
        var service = helper.GetService<IDeviceSecurity>();
        await service.UpdateToken(GenerateToken(helper, new List<Claim>
        {
            new(ClaimsValues.ClaimTypes.Field, ClaimsValues.ClaimValues.Auditor),
            new(ClaimsValues.ClaimTypes.Field, ClaimsValues.ClaimValues.Executive),
            new(ClaimsValues.ClaimTypes.Sales, ClaimsValues.ClaimValues.Auditor)
        }));

        var result = await helper.GetService<ISecurity>().GetAllowedPages();

        Assert.Equal(2, result.Count());
    }

    private static string GenerateToken(HostTestHelper testingService, IEnumerable<Claim>? claims = null)
    {
        var ci = new ClaimsIdentity("xxx");
        if (claims is null)
            ci.AddClaim(new Claim("bla1", "bla2"));
        else
            ci.AddClaims(claims);

        var key = Encoding.ASCII.GetBytes(new string('x', 30));
        var token = testingService.GetService<IJwtService>().GenerateToken(new SecurityTokenDescriptor
        {
            Subject = ci,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        return token;
    }
}