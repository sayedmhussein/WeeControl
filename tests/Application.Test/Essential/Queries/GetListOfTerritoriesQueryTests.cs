using System.Collections.Generic;
using System.Linq;
using WeeControl.Application.Essential.Queries;
using WeeControl.Domain.Contexts.Essential;
using Xunit;

namespace WeeControl.Application.Test.Essential.Queries;

public class GetListOfTerritoriesQueryTests
{
    [Fact]
    public async void WhenQueringWithNullArgument_ReturnAllTerritoreis()
    {
        using var testHelper = new TestHelper();
        await testHelper.EssentialDb.Territories.AddRangeAsync(new List<TerritoryDbo>()
        {
            TerritoryDbo.Create("coa", null, "ccc", "name"),
            TerritoryDbo.Create("cob", "coa", "ccc", "name"), 
            TerritoryDbo.Create("coc", "cob", "ccc", "name")
        });
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var handler = new GetListOfTerritoriesQuery.GetListOfTerritoriesHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);

        var list = await handler.Handle(new GetListOfTerritoriesQuery(), default);
        
        Assert.Equal(3, list.Payload.Count());
    }
    
    [Fact]
    public async void WhenQueringWithInvalidCode_ReturnEmpty()
    {
        using var testHelper = new TestHelper();
        await testHelper.EssentialDb.Territories.AddRangeAsync(new List<TerritoryDbo>()
        {
            TerritoryDbo.Create("coa", null, "ccc", "name"),
            TerritoryDbo.Create("cob", "coa", "ccc", "name"), 
            TerritoryDbo.Create("coc", "cob", "ccc", "name")
        });
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var handler = new GetListOfTerritoriesQuery.GetListOfTerritoriesHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);

        var list = await handler.Handle(new GetListOfTerritoriesQuery("coz"), default);
        
        Assert.Empty(list.Payload);
    }
    
    [Fact]
    public async void WhenQueringWithChild_ReturnAllTerritoreisUntilChild()
    {
        using var testHelper = new TestHelper();
        await testHelper.EssentialDb.Territories.AddRangeAsync(new List<TerritoryDbo>()
        {
            TerritoryDbo.Create("coa", null, "ccc", "name"),
            TerritoryDbo.Create("cob", "coa", "ccc", "name"), 
            TerritoryDbo.Create("coc", "cob", "ccc", "name")
        });
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var handler = new GetListOfTerritoriesQuery.GetListOfTerritoriesHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);

        var list = await handler.Handle(new GetListOfTerritoriesQuery("cob"), default);
        
        Assert.Equal(2, list.Payload.Count());
    }
    
    [Fact]
    public async void WhenQueringWithChild_ReturnAllTerritoreisUntilChild2()
    {
        using var testHelper = new TestHelper();
        await testHelper.EssentialDb.Territories.AddRangeAsync(new List<TerritoryDbo>()
        {
            TerritoryDbo.Create("coa", null, "ccc", "name"),
            TerritoryDbo.Create("cob", "coa", "ccc", "name"), 
            TerritoryDbo.Create("coc", "cob", "ccc", "name"), 
            TerritoryDbo.Create("cod", "coc", "ccc", "name"), 
            TerritoryDbo.Create("coe", null, "ccc", "name"), 
            TerritoryDbo.Create("cof", "cod", "ccc", "name")
        });
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var handler = new GetListOfTerritoriesQuery.GetListOfTerritoriesHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);

        var list = await handler.Handle(new GetListOfTerritoriesQuery("coc"), default);
        
        Assert.Equal(3, list.Payload.Count());
    }
    
    [Fact]
    public async void WhenQueringWithChild_ReturnAllTerritoreisUntilChild3()
    {
        using var testHelper = new TestHelper();
        await testHelper.EssentialDb.Territories.AddRangeAsync(new List<TerritoryDbo>()
        {
            TerritoryDbo.Create("coa", null, "ccc", "name"),
            TerritoryDbo.Create("cob", "coa", "ccc", "name"), 
            TerritoryDbo.Create("coc", "cob", "ccc", "name"), 
            TerritoryDbo.Create("cod", "coc", "ccc", "name"), 
            TerritoryDbo.Create("coe", null, "ccc", "name"), 
            TerritoryDbo.Create("cof", "cod", "ccc", "name"),
            TerritoryDbo.Create("cog", "coe", "ccc", "name")
        });
        await testHelper.EssentialDb.SaveChangesAsync(default);
        var handler = new GetListOfTerritoriesQuery.GetListOfTerritoriesHandler(testHelper.EssentialDb, testHelper.CurrentUserInfoMock.Object);

        var l = new List<string>() { "coc", "cog" };
        var list = await handler.Handle(new GetListOfTerritoriesQuery(l), default);
        
        Assert.Equal(4, list.Payload.Count());
    }
}