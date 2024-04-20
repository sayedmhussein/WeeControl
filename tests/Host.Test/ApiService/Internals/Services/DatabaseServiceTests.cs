using System.Net;
using WeeControl.Core.DomainModel.Essentials.Dto;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.Test.ApiService.Internals.Services;

public class DatabaseServiceTests
{
    [Fact]
    public async void DatabaseServiceTest()
    {
        using var helper = new HostTestHelper(HttpStatusCode.OK);
        var service = helper.GetService<IDatabaseService>();

        var dbo = LoginRequestDto.Create("email@mail.mail", "password");
        await service.AddToTable(dbo);
        Assert.NotEmpty(await service.ReadFromTable<LoginRequestDto>());

        var saved =
            (await service.ReadFromTable<LoginRequestDto>()).First(x => x.UsernameOrEmail == dbo.UsernameOrEmail);
        Assert.Equal(dbo.UsernameOrEmail, saved.UsernameOrEmail);
        Assert.Equal(dbo.Password, saved.Password);

        await service.ClearTable<LoginRequestDto>();
        Assert.Empty(await service.ReadFromTable<LoginRequestDto>());
    }

    [Fact]
    public async void DatabaseServiceDeleteTest()
    {
        using var helper = new HostTestHelper(HttpStatusCode.OK);
        var service = helper.GetService<IDatabaseService>();

        var dbo = LoginRequestDto.Create("email@mail.mail", "password");
        await service.AddToTable(dbo);
        Assert.NotEmpty(await service.ReadFromTable<LoginRequestDto>());

        var saved =
            (await service.ReadFromTable<LoginRequestDto>()).First(x => x.UsernameOrEmail == dbo.UsernameOrEmail);
        Assert.Equal(dbo.UsernameOrEmail, saved.UsernameOrEmail);
        Assert.Equal(dbo.Password, saved.Password);

        await service.ClearAllTables();
        Assert.Empty(await service.ReadFromTable<LoginRequestDto>());
    }
}