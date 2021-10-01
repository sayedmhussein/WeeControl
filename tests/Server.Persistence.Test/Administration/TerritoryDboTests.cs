using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WeeControl.Server.Domain.Administration;
using WeeControl.Server.Domain.Administration.Entities;
using WeeControl.Server.Domain.Administration.ValueObjects;
using WeeControl.Server.Persistence.Administration;
using Xunit;

namespace WeeControl.Server.Persistence.Test.Administration
{
    public class TerritoryDboTests : IDisposable
    {
        private IAdministrationDbContext context;

        public TerritoryDboTests()
        {
            var options = new DbContextOptionsBuilder<AdministrationDbContext>();
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
            options.UseInMemoryDatabase(nameof(TerritoryDboTests));
            options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            
            context = new AdministrationDbContext(options: options.Options);
        }

        public void Dispose()
        {
            context = null;
        }

        [Fact]
        public async void WhenQueryingTerritories_ListNotEmpty()
        {
            var territories = await context.Territories.ToListAsync();

            Assert.NotEmpty(territories);
        }

        [Fact]
        public async void WhenAddingTerritory_IdHasNewCode()
        {
            var territory = Territory.Create("WDC-US", "OtherName", "???", new Address());

            await context.Territories.AddAsync(territory);
            await context.SaveChangesAsync(default);

            Assert.NotEmpty(territory.TerritoryCode);
        }

        

        
    }
}
