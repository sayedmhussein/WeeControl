using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MySystem.Application.Common.Exceptions;
using MySystem.Application.Common.Interfaces;
using MySystem.Application.Territory.Query.GetTerritories;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using MySystem.Domain.EntityDbo.PublicSchema;
using MySystem.Persistence;
using MySystem.SharedKernel.Entities.Territory.V1Dto;
using Xunit;

namespace MySystem.Application.Test.Territory.Query.GetTerritories
{
    public class GetTerritoriesV1HandlerTesters : IDisposable
    {
        private IMySystemDbContext dbContext;

        public GetTerritoriesV1HandlerTesters()
        {
            dbContext = new ServiceCollection().AddPersistenceAsInMemory(new Random().NextDouble().ToString()).BuildServiceProvider().GetService<IMySystemDbContext>();
        }

        public void Dispose()
        {
            dbContext = null;
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesByEmployeeId_ReturnListOfEmployeeTerritoresDtoType()
        {
            var admin = dbContext.Employees.FirstOrDefault();

            var responseDto = await new GetTerritoriesV1Handler(dbContext).Handle(new GetTerritoriesV1Query() { EmployeeId = admin.Id }, default);

            Assert.IsType<List<TerritoryDto>>(responseDto);
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesByEmployeeId_ReturnSingleOffice()
        {
            var admin = dbContext.Employees.FirstOrDefault();

            var responseDto = await new GetTerritoriesV1Handler(dbContext).Handle(new GetTerritoriesV1Query() { EmployeeId = admin.Id }, default);

            Assert.Single(responseDto);
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesBySessionId_ReturnSingleOffice()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            var session = new EmployeeSessionDbo() { Employee = admin, DeviceId = "device" };
            await dbContext.EmployeeSessions.AddAsync(session);
            await dbContext.SaveChangesAsync(default);

            var responseDto = await new GetTerritoriesV1Handler(dbContext).Handle(new GetTerritoriesV1Query() { SessionId = session.Id }, default);

            Assert.Single(responseDto);
        }

        [Fact(Skip = "This feature not implemented yet!")]
        public async void WhenGettingTerritoriesByTerritoryId_ReturnSingleOffice()
        {
            var territory = dbContext.Territories.FirstOrDefault();

            var responseDto = await new GetTerritoriesV1Handler(dbContext).Handle(new GetTerritoriesV1Query() { TerritoryId = territory.Id }, default);

            Assert.Single(responseDto);
        }

        [Fact]
        public async void WhenGettingUnknownEmployeeTerritoriesByEmployeeId_ThrowNotFoundException()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () => await new GetTerritoriesV1Handler(dbContext).Handle(new GetTerritoriesV1Query() { EmployeeId = Guid.NewGuid() }, default));
        }

        [Fact]
        public async void WhenGettingUnknownEmployeeTerritoriesBySessionId_ThrowNotFoundException()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () => await new GetTerritoriesV1Handler(dbContext).Handle(new GetTerritoriesV1Query() { SessionId = Guid.NewGuid() }, default));
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesByEmployeeIdWhichHasOneMoreTerritory_ReturnDoubleTerritories()
        {
            var territory = dbContext.Territories.FirstOrDefault();
            await dbContext.Territories.AddAsync(new TerritoryDbo() { ReportTo = territory, CountryId = "sss", Name = "name" });
            await dbContext.SaveChangesAsync(default);
            //
            var admin = dbContext.Employees.FirstOrDefault();
            
            var responseDto = await new GetTerritoriesV1Handler(dbContext).Handle(new GetTerritoriesV1Query() { EmployeeId = admin.Id }, default);

            Assert.Equal(2, responseDto.Count());
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesByEmployeeIdWhichHasTwoMoreTerritoryOneLevelDown_ReturnTripleTerritories()
        {
            var territory = dbContext.Territories.FirstOrDefault();
            await dbContext.Territories.AddAsync(new TerritoryDbo() { ReportTo = territory, CountryId = "sss", Name = "name1" });
            await dbContext.Territories.AddAsync(new TerritoryDbo() { ReportTo = territory, CountryId = "sss", Name = "name2" });
            await dbContext.SaveChangesAsync(default);
            //
            var admin = dbContext.Employees.FirstOrDefault();

            var responseDto = await new GetTerritoriesV1Handler(dbContext).Handle(new GetTerritoriesV1Query() { EmployeeId = admin.Id }, default);

            Assert.Equal(3, responseDto.Count());
        }

        [Fact(Skip = "This test produce a known bug, it will be solved soon.")]
        public async void WhenGettingAdminTerritoriesByEmployeeIdWhichHasTwoMoreTerritoryTwoLevelsDown_ReturnTripleTerritories()
        {
            var territory1 = dbContext.Territories.FirstOrDefault();
            //
            var territory2 = new TerritoryDbo() { ReportTo = territory1, CountryId = "sss", Name = "name1" };
            await dbContext.Territories.AddAsync(territory2);
            await dbContext.SaveChangesAsync(default);
            //
            var territory3 = new TerritoryDbo() { ReportTo = territory2, CountryId = "sss", Name = "name1" };
            await dbContext.Territories.AddAsync(territory3);
            await dbContext.SaveChangesAsync(default);
            //
            var admin = dbContext.Employees.FirstOrDefault();

            var responseDto = await new GetTerritoriesV1Handler(dbContext).Handle(new GetTerritoriesV1Query() { EmployeeId = admin.Id }, default);

            Assert.Equal(3, responseDto.Count());
        }
    }
}
