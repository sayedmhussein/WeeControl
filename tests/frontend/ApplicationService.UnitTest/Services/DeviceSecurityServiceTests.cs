using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WeeControl.Core.SharedKernel.Interfaces;
using WeeControl.Frontend.AppService.DeviceInterfaces;
using WeeControl.Frontend.AppService.Internals.Services;

namespace WeeControl.Frontend.Service.UnitTest.Services;

public class DeviceSecurityServiceTests : IDisposable
{
    private Mock<IStorage> storageMock;
    private IJwtService jwtService;

    public DeviceSecurityServiceTests()
    {
        storageMock = new Mock<IStorage>();

        jwtService = new JwtService();
    }

    public void Dispose()
    {
        storageMock = null;
    }

    [Theory]
    [InlineData("Token", true)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public async void IsAuthenticatedTests(string token, bool isTrue)
    {
        storageMock.Setup(x => x.GetKeyValue(It.IsAny<string>())).ReturnsAsync(token);

        var result = await new SecurityService(storageMock.Object, jwtService).IsAuthenticatedAsync();

        Assert.Equal(isTrue, result);
    }

    [Fact]
    public async void TokenSettingAndGetting()
    {
        storageMock.Setup(x => x.SaveKeyValue(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string k, string v) => storageMock.Setup(x => x.GetKeyValue(k)).ReturnsAsync(v));

        const string token = "This should contains Token String";
        var service = new SecurityService(storageMock.Object, jwtService);
        await service.UpdateTokenAsync(token);

        Assert.Equal(token, await service.GetTokenAsync());
    }

    [Fact]
    public async void DeletingTokenTest()
    {
        storageMock.Setup(x => x.SaveKeyValue(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string k, string v) => storageMock.Setup(x => x.GetKeyValue(k)).ReturnsAsync(v));
        const string token = "This should contains Token String";

        var service = new SecurityService(storageMock.Object, jwtService);
        await service.UpdateTokenAsync(token);
        await service.DeleteTokenAsync();

        Assert.NotEqual(token, await service.GetTokenAsync());
    }

    [Fact]
    public async void GettingClaimPrincipleTest()
    {
        storageMock.Setup(x => x.SaveKeyValue(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string k, string v) => storageMock.Setup(x => x.GetKeyValue(k)).ReturnsAsync(v));
        var ci = new ClaimsIdentity("xxx");
        ci.AddClaim(new Claim("bla1", "bla2"));
        var key = Encoding.ASCII.GetBytes(new string('x', 30));
        var token = jwtService.GenerateToken(new SecurityTokenDescriptor()
        {
            Subject = ci,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        var service = new SecurityService(storageMock.Object, jwtService);
        await service.UpdateTokenAsync(token);
        var cp = await service.GetClaimsPrincipal();

        Assert.Equal("bla1", cp.Claims.First().Type);
        Assert.Equal("bla2", cp.Claims.First().Value);
    }
}