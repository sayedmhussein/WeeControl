using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MySystem.Application.Common.Exceptions;
using MySystem.Application.Common.Interfaces;
using MySystem.Application.Territory.Command.AddTerritory;
using MySystem.Persistence;
using MySystem.SharedKernel.Entities.Territory.V1Dto;
using Xunit;

namespace MySystem.Application.Test.Territory.Command.AddTerritory
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
            await Assert.ThrowsAsync<BadRequestException>(async () => await new AddTerritoryV1Handler(dbContext).Handle(new AddTerritoryV1Command() { TerritoryDtos = null }, default));
        }

        [Fact]
        public async void WhenEmptyCommandPayload_ThrowBadRequest()
        {
            await Assert.ThrowsAsync<BadRequestException>(async () => await new AddTerritoryV1Handler(dbContext).Handle(new AddTerritoryV1Command() { TerritoryDtos = new List<TerritoryDto>() }, default));
        }

        [Fact]
        public async void WhenAddingNewTerritory_ReturnSameTerritoryWithNewId()
        {
            var territory = new TerritoryDto() { Name = "Test Territory", CountryId = "usa" };

            var response = await new AddTerritoryV1Handler(dbContext).Handle(new AddTerritoryV1Command() { TerritoryDtos = new List<TerritoryDto>() { territory } }, default);

            Assert.NotNull(response.FirstOrDefault(x => x.Name == "Test Territory").Id);
        }

        [Fact]
        public async void WhenAddingNewTerritoryInSameCountryWithSameName_ReturnSameTerritoryWithNewId()
        {
            var territory = new TerritoryDto() { Name = "Test Territory", CountryId = "usa" };

            var response = await new AddTerritoryV1Handler(dbContext).Handle(new AddTerritoryV1Command() { TerritoryDtos = new List<TerritoryDto>() { territory } }, default);

            Assert.NotNull(response.FirstOrDefault(x => x.Name == "Test Territory").Id);
        }
    }
}
