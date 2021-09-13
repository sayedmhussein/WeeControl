using System;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Domain.Common.Interfaces;
using WeeControl.Backend.Persistence;

namespace WeeControl.Backend.Application.Test.EntityGroup.Territory.V1.Commands
{
    public class AddTerritoryV1HandlerTesters : IDisposable
    {
        private IMySystemDbContext dbContext;

        public AddTerritoryV1HandlerTesters()
        {
            dbContext = new ServiceCollection().AddPersistenceAsInMemory(new Random().NextDouble().ToString()).BuildServiceProvider().GetService<IMySystemDbContext>();
        }

        public void Dispose()
        {
            dbContext = null;
        }

        //[Fact]
        //public async void WhenNullCommandPayload_ThrowBadRequest()
        //{
        //    await Assert.ThrowsAsync<BadRequestException>(async () => await new UpdateTerritoryHandler(dbContext).Handle(new UpdateTerritoryCommand() { TerritoryDto = null }, default));
        //}

        //[Fact]
        //public async void WhenEmptyCommandPayload_ThrowBadRequest()
        //{
        //    await Assert.ThrowsAsync<BadRequestException>(async () => await new UpdateTerritoryHandler(dbContext).Handle(new UpdateTerritoryCommand() { TerritoryDto = new TerritoryDto() }, default));
        //}

        //[Fact]
        //public async void WhenAddingNewTerritory_ReturnSameTerritoryWithNewId()
        //{
        //    var territory = new TerritoryDto() { Name = "Test Territory", CountryId = "usa" };

        //    var response = await new UpdateTerritoryHandler(dbContext).Handle(new UpdateTerritoryCommand() { TerritoryDto =  territory  }, default);

        //    Assert.NotNull(response.FirstOrDefault(x => x.Name == "Test Territory").Id);
        //}

        //[Fact]
        //public async void WhenAddingNewTerritoryInSameCountryWithSameName_ReturnSameTerritoryWithNewId()
        //{
        //    var territory = new TerritoryDto() { Name = "Test Territory", CountryId = "usa" };

        //    var response = await new UpdateTerritoryHandler(dbContext).Handle(new UpdateTerritoryCommand() { TerritoryDto = territory  }, default);

        //    Assert.NotNull(response.FirstOrDefault(x => x.Name == "Test Territory").Id);
        //}
    }
}
