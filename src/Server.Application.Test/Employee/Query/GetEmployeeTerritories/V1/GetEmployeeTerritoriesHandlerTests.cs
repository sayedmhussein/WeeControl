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

namespace MySystem.Application.Test.Employee.Query.GetEmployeeTerritories.V1
{
    public class GetEmployeeTerritoriesHandlerTests : IDisposable
    {
        private IMySystemDbContext dbContext;
        private EmployeeDbo randomEmployeeDbo;

        public GetEmployeeTerritoriesHandlerTests()
        {
            dbContext = new ServiceCollection().AddPersistenceAsInMemory(null).BuildServiceProvider().GetService<IMySystemDbContext>();

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
        public async void WhenGettingAdminOffice_ReturnListOfOffices()
        {
            randomEmployeeDbo.TerritoryId = dbContext.Territories.FirstOrDefault().Id;
            await dbContext.Employees.AddAsync(randomEmployeeDbo);
            await dbContext.SaveChangesAsync(default);

            var responseDto = await new GetEmployeeTerritoriesHandler(dbContext).Handle(new GetEmployeeTerritoriesQuery() { EmployeeId = randomEmployeeDbo.Id }, default);

            Assert.NotNull(responseDto);

            Assert.Single(responseDto.Payload);
        }

        [Fact]
        public async void WhenGettingUnknownEmployeeTerritory_ThrowNotFoundException()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () => await new GetEmployeeTerritoriesHandler(dbContext).Handle(new GetEmployeeTerritoriesQuery() { EmployeeId = Guid.NewGuid() }, default));
        }

        [Fact]
        public async void WhenGettingOtherEmployeeTerritoryWhichReportToHeadOffice_ReturnTwoOffices()
        {
            var adminEmployee = dbContext.Employees.FirstOrDefault();
            var childTerritory = new TerritoryDbo() { CountryId = "bla", OfficeName = "BlaBla", ReportToId = dbContext.Territories.FirstOrDefault().Id };
            await dbContext.Territories.AddAsync(childTerritory);
            await dbContext.SaveChangesAsync(default);

            randomEmployeeDbo.TerritoryId = childTerritory.Id;
            await dbContext.Employees.AddAsync(randomEmployeeDbo);
            await dbContext.SaveChangesAsync(default);

            var responseDto = await new GetEmployeeTerritoriesHandler(dbContext).Handle(new GetEmployeeTerritoriesQuery() { EmployeeId = adminEmployee.Id }, default);

            Assert.NotNull(responseDto);

            Assert.Equal(2, responseDto.Payload.Count());
        }
    }
}
