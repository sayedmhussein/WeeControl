using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Essential;
using WeeControl.Application.Essential.Queries;
using WeeControl.Application.Interfaces;
using WeeControl.Domain.Essential.Entities;
using WeeControl.Persistence;
using Xunit;

namespace WeeControl.Application.Test.EssentialContext.Queries;

public class GetListOfTerritoriesQueryTests : IDisposable
{
    private Mock<ICurrentUserInfo> currentUserInfoMock;
    private IEssentialDbContext context;
    
    public GetListOfTerritoriesQueryTests()
    {
        currentUserInfoMock = new Mock<ICurrentUserInfo>();
        context = new ServiceCollection().AddPersistenceAsInMemory().BuildServiceProvider().GetService<IEssentialDbContext>();
    }

    public void Dispose()
    {
        currentUserInfoMock = null;
        context = null;
    }

    [Fact]
    public async void WhenQueringWithNullArgument_ReturnAllTerritoreis()
    {
        await context.Territories.AddRangeAsync(new List<TerritoryDbo>()
        {
            TerritoryDbo.Create("coa", null, "ccc", "name"),
            TerritoryDbo.Create("cob", "coa", "ccc", "name"), 
            TerritoryDbo.Create("coc", "cob", "ccc", "name")
        });
        await context.SaveChangesAsync(default);
        var handler = new GetListOfTerritoriesQuery.GetListOfTerritoriesHandler(context, currentUserInfoMock.Object);

        var list = await handler.Handle(new GetListOfTerritoriesQuery(), default);
        
        Assert.Equal(3, list.Payload.Count());
    }
    
    [Fact]
    public async void WhenQueringWithInvalidCode_ReturnEmpty()
    {
        await context.Territories.AddRangeAsync(new List<TerritoryDbo>()
        {
            TerritoryDbo.Create("coa", null, "ccc", "name"),
            TerritoryDbo.Create("cob", "coa", "ccc", "name"), 
            TerritoryDbo.Create("coc", "cob", "ccc", "name")
        });
        await context.SaveChangesAsync(default);
        var handler = new GetListOfTerritoriesQuery.GetListOfTerritoriesHandler(context, currentUserInfoMock.Object);

        var list = await handler.Handle(new GetListOfTerritoriesQuery("coz"), default);
        
        Assert.Empty(list.Payload);
    }
    
    [Fact]
    public async void WhenQueringWithChild_ReturnAllTerritoreisUntilChild()
    {
        await context.Territories.AddRangeAsync(new List<TerritoryDbo>()
        {
            TerritoryDbo.Create("coa", null, "ccc", "name"),
            TerritoryDbo.Create("cob", "coa", "ccc", "name"), 
            TerritoryDbo.Create("coc", "cob", "ccc", "name")
        });
        await context.SaveChangesAsync(default);
        var handler = new GetListOfTerritoriesQuery.GetListOfTerritoriesHandler(context, currentUserInfoMock.Object);

        var list = await handler.Handle(new GetListOfTerritoriesQuery("cob"), default);
        
        Assert.Equal(2, list.Payload.Count());
    }
    
    [Fact]
    public async void WhenQueringWithChild_ReturnAllTerritoreisUntilChild2()
    {
        await context.Territories.AddRangeAsync(new List<TerritoryDbo>()
        {
            TerritoryDbo.Create("coa", null, "ccc", "name"),
            TerritoryDbo.Create("cob", "coa", "ccc", "name"), 
            TerritoryDbo.Create("coc", "cob", "ccc", "name"), 
            TerritoryDbo.Create("cod", "coc", "ccc", "name"), 
            TerritoryDbo.Create("coe", null, "ccc", "name"), 
            TerritoryDbo.Create("cof", "cod", "ccc", "name")
        });
        await context.SaveChangesAsync(default);
        var handler = new GetListOfTerritoriesQuery.GetListOfTerritoriesHandler(context, currentUserInfoMock.Object);

        var list = await handler.Handle(new GetListOfTerritoriesQuery("coc"), default);
        
        Assert.Equal(3, list.Payload.Count());
    }
    
    [Fact]
    public async void WhenQueringWithChild_ReturnAllTerritoreisUntilChild3()
    {
        await context.Territories.AddRangeAsync(new List<TerritoryDbo>()
        {
            TerritoryDbo.Create("coa", null, "ccc", "name"),
            TerritoryDbo.Create("cob", "coa", "ccc", "name"), 
            TerritoryDbo.Create("coc", "cob", "ccc", "name"), 
            TerritoryDbo.Create("cod", "coc", "ccc", "name"), 
            TerritoryDbo.Create("coe", null, "ccc", "name"), 
            TerritoryDbo.Create("cof", "cod", "ccc", "name"),
            TerritoryDbo.Create("cog", "coe", "ccc", "name")
        });
        await context.SaveChangesAsync(default);
        var handler = new GetListOfTerritoriesQuery.GetListOfTerritoriesHandler(context, currentUserInfoMock.Object);

        var l = new List<string>() { "coc", "cog" };
        var list = await handler.Handle(new GetListOfTerritoriesQuery(l), default);
        
        Assert.Equal(4, list.Payload.Count());
    }
}