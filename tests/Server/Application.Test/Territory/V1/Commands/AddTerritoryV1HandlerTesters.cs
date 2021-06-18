using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Server.Application.Common.Exceptions;
using WeeControl.Server.Application.Common.Interfaces;
using WeeControl.Server.Application.Territory.V1.Commands;
using WeeControl.Server.Application.Territory.V1.Handlers;
using WeeControl.Server.Persistence;
using WeeControl.SharedKernel.BasicSchemas.Territory.DtosV1;
using Xunit;

namespace WeeControl.Server.Application.Test.Territory.V1.Commands
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

        [Fact]
        public async void WhenNullCommandPayload_ThrowBadRequest()
        {
            await Assert.ThrowsAsync<BadRequestException>(async () => await new AddOrEditTerritoriesHandler(dbContext).Handle(new AddOrEditTerritoriesCommand() { TerritoryDtos = null }, default));
        }

        [Fact]
        public async void WhenEmptyCommandPayload_ThrowBadRequest()
        {
            await Assert.ThrowsAsync<BadRequestException>(async () => await new AddOrEditTerritoriesHandler(dbContext).Handle(new AddOrEditTerritoriesCommand() { TerritoryDtos = new List<TerritoryDto>() }, default));
        }

        [Fact]
        public async void WhenAddingNewTerritory_ReturnSameTerritoryWithNewId()
        {
            var territory = new TerritoryDto() { Name = "Test Territory", CountryId = "usa" };

            var response = await new AddOrEditTerritoriesHandler(dbContext).Handle(new AddOrEditTerritoriesCommand() { TerritoryDtos = new List<TerritoryDto>() { territory } }, default);

            Assert.NotNull(response.FirstOrDefault(x => x.Name == "Test Territory").Id);
        }

        [Fact]
        public async void WhenAddingNewTerritoryInSameCountryWithSameName_ReturnSameTerritoryWithNewId()
        {
            var territory = new TerritoryDto() { Name = "Test Territory", CountryId = "usa" };

            var response = await new AddOrEditTerritoriesHandler(dbContext).Handle(new AddOrEditTerritoriesCommand() { TerritoryDtos = new List<TerritoryDto>() { territory } }, default);

            Assert.NotNull(response.FirstOrDefault(x => x.Name == "Test Territory").Id);
        }
    }
}
