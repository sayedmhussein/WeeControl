// using System;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
// using WeeControl.Backend.Domain.BoundedContexts.HumanResources.TerritoryModule.Entities;
// using WeeControl.Backend.Domain.BoundedContexts.HumanResources.TerritoryModule.ValueObjects;
// using WeeControl.Backend.Persistence;
// using Xunit;
//
// namespace WeeControl.Server.Persistence.Test.BoundedContexts.HumanResources
// {
//     public class TerritoryDboTests : IDisposable
//     {
//         private IHumanResourcesDbContext context;
//
//         public TerritoryDboTests()
//         {
//             context = new ServiceCollection().AddPersistenceAsInMemory(nameof(TerritoryDboTests)).BuildServiceProvider().GetService<IHumanResourcesDbContext>();
//         }
//
//         public void Dispose()
//         {
//             context = null;
//         }
//
//         [Fact]
//         public async void WhenQueryingTerritories_ListNotEmpty()
//         {
//             var territories = await context.Territories.ToListAsync();
//
//             Assert.NotEmpty(territories);
//         }
//
//         [Fact]
//         public async void WhenAddingTerritory_IdHasNewCode()
//         {
//             var territory = Territory.Create("WDC-US", "OtherName", "???", new Address());
//
//             await context.Territories.AddAsync(territory);
//             await context.SaveChangesAsync(default);
//
//             Assert.NotEmpty(territory.TerritoryCode);
//         }
//
//         
//
//         
//     }
// }
