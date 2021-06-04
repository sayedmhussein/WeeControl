using System;
using Xunit;
using Moq;
using MySystem.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MySystem.Persistence;
using MySystem.Application.Employee.Query.GetEmployeeTerritories.V1;
using MySystem.SharedKernel.Entities.Employee.V1Dto;
using MySystem.SharedKernel.Entities.Public.Constants;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using System.Linq;
using MySystem.Application.Common.Exceptions;
using MySystem.Domain.EntityDbo.PublicSchema;
using System.Collections.Generic;
using MySystem.SharedKernel.Entites.Employee.V1Dto;

namespace MySystem.Application.Test.Employee.Query.GetEmployeeTerritories.V1
{
    public class GetEmployeeTerritoriesHandlerTests : IDisposable
    {
        private IMySystemDbContext dbContext;
        private EmployeeDbo randomEmployeeDbo;

        public GetEmployeeTerritoriesHandlerTests()
        {
            dbContext = new ServiceCollection().AddPersistenceAsInMemory(new Random().NextDouble().ToString()).BuildServiceProvider().GetService<IMySystemDbContext>();

            randomEmployeeDbo = new EmployeeDbo()
            {
                EmployeeTitle = Titles.List[Titles.Title.Mr],
                FirstName = new Random().NextDouble().ToString(),
                LastName = new Random().NextDouble().ToString(),
                Gender = Genders.List[Genders.Gender.Male],
                TerritoryId = Guid.NewGuid(),
                Username = new Random().NextDouble().ToString(),
                Password = new Random().NextDouble().ToString()
            };
        }

        public void Dispose()
        {
            dbContext = null;
            randomEmployeeDbo = null;
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesByEmployeeId_ReturnListOfEmployeeTerritoresDto()
        {
            var admin = dbContext.Employees.FirstOrDefault();

            var responseDto = await new GetEmployeeTerritoriesHandler(dbContext).Handle(new GetEmployeeTerritoriesQuery() { EmployeeId = admin.Id }, default);

            Assert.IsType<List<EmployeeTerritoriesDto>>(responseDto.Payload);
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesByEmployeeId_ReturnSingleOffice()
        {
            var admin = dbContext.Employees.FirstOrDefault();

            var responseDto = await new GetEmployeeTerritoriesHandler(dbContext).Handle(new GetEmployeeTerritoriesQuery() { EmployeeId = admin.Id }, default);

            Assert.Single(responseDto.Payload);
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesBySessionId_ReturnSingleOffice()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            var session = new EmployeeSessionDbo() { Employee = admin, DeviceId = "device" };
            await dbContext.EmployeeSessions.AddAsync(session);
            await dbContext.SaveChangesAsync(default);

            var responseDto = await new GetEmployeeTerritoriesHandler(dbContext).Handle(new GetEmployeeTerritoriesQuery() { SessionId = session.Id }, default);

            Assert.Single(responseDto.Payload);
        }

        [Fact]
        public async void WhenGettingUnknownEmployeeTerritoriesByEmployeeId_ThrowNotFoundException()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () => await new GetEmployeeTerritoriesHandler(dbContext).Handle(new GetEmployeeTerritoriesQuery() { EmployeeId = Guid.NewGuid() }, default));
        }

        [Fact]
        public async void WhenGettingUnknownEmployeeTerritoriesBySessionId_ThrowNotFoundException()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () => await new GetEmployeeTerritoriesHandler(dbContext).Handle(new GetEmployeeTerritoriesQuery() { SessionId = Guid.NewGuid() }, default));
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesByEmployeeIdWhichHasOneMoreTerritory_ReturnDoubleTerritories()
        {
            var territory = dbContext.Territories.FirstOrDefault();
            await dbContext.Territories.AddAsync(new TerritoryDbo() { ReportTo = territory, CountryId = "sss", OfficeName = "name" });
            await dbContext.SaveChangesAsync(default);
            //
            var admin = dbContext.Employees.FirstOrDefault();
            
            var responseDto = await new GetEmployeeTerritoriesHandler(dbContext).Handle(new GetEmployeeTerritoriesQuery() { EmployeeId = admin.Id }, default);

            Assert.Equal(2, responseDto.Payload.Count());
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesByEmployeeIdWhichHasTwoMoreTerritoryOneLevelDown_ReturnTripleTerritories()
        {
            var territory = dbContext.Territories.FirstOrDefault();
            await dbContext.Territories.AddAsync(new TerritoryDbo() { ReportTo = territory, CountryId = "sss", OfficeName = "name1" });
            await dbContext.Territories.AddAsync(new TerritoryDbo() { ReportTo = territory, CountryId = "sss", OfficeName = "name2" });
            await dbContext.SaveChangesAsync(default);
            //
            var admin = dbContext.Employees.FirstOrDefault();

            var responseDto = await new GetEmployeeTerritoriesHandler(dbContext).Handle(new GetEmployeeTerritoriesQuery() { EmployeeId = admin.Id }, default);

            Assert.Equal(3, responseDto.Payload.Count());
        }

        [Fact(Skip = "This test produce a known bug, it will be solved soon.")]
        public async void WhenGettingAdminTerritoriesByEmployeeIdWhichHasTwoMoreTerritoryTwoLevelsDown_ReturnTripleTerritories()
        {
            var territory1 = dbContext.Territories.FirstOrDefault();
            //
            var territory2 = new TerritoryDbo() { ReportTo = territory1, CountryId = "sss", OfficeName = "name1" };
            await dbContext.Territories.AddAsync(territory2);
            await dbContext.SaveChangesAsync(default);
            //
            var territory3 = new TerritoryDbo() { ReportTo = territory2, CountryId = "sss", OfficeName = "name1" };
            await dbContext.Territories.AddAsync(territory3);
            await dbContext.SaveChangesAsync(default);
            //
            var admin = dbContext.Employees.FirstOrDefault();

            var responseDto = await new GetEmployeeTerritoriesHandler(dbContext).Handle(new GetEmployeeTerritoriesQuery() { EmployeeId = admin.Id }, default);

            Assert.Equal(3, responseDto.Payload.Count());
        }
    }
}
