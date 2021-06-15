using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MySystem.Application.Common.Exceptions;
using MySystem.Application.Common.Interfaces;
using MySystem.Application.Territory.Command.DeleteTerritories;
using MySystem.Persistence;
using MySystem.SharedKernel.Enumerators.Employee;
using MySystem.SharedKernel.Interfaces.Values;
using MySystem.SharedKernel.Services;
using Xunit;

namespace MySystem.Application.Test.Territory.Command.DeleteTerritory
{
    public class DeleteTerritoryV1HandlerTesters : IDisposable
    {
        private IMySystemDbContext dbContext;
        private Mock<ICurrentUserInfo> userInfoMock;
        private readonly IEmployeeValues employeeValues = new EmployeeValues();
        private readonly ITerritoryValues values = new TerritoryValues();

        public DeleteTerritoryV1HandlerTesters()
        {
            dbContext = new ServiceCollection().AddPersistenceAsInMemory(new Random().NextDouble().ToString()).BuildServiceProvider().GetService<IMySystemDbContext>();

            userInfoMock = new Mock<ICurrentUserInfo>();
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { dbContext.Employees.FirstOrDefault().Id });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(employeeValues.ClaimType[ClaimTypeEnum.HumanResources], employeeValues.ClaimTag[ClaimTagEnum.Delete]) });
        }

        public void Dispose()
        {
            dbContext = null;
            userInfoMock = null;
        }

        #region Constructor Unit Tests
        [Fact]
        public void WhenDbContextIsNull_ThrowsArgumentNullException()
        {
            userInfoMock = new Mock<ICurrentUserInfo>();

            Assert.Throws<ArgumentNullException>(() => new DeleteTerritoriesV1Handler(null, userInfoMock.Object, values, employeeValues));
        }

        [Fact]
        public void WhenUserInfoIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteTerritoriesV1Handler(dbContext, null, values, employeeValues));
        }
        #endregion

        #region Handle Parameters Unit Tests
        [Fact]
        public async void WhenCommandIsNull_ThrowArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await new DeleteTerritoriesV1Handler(dbContext, userInfoMock.Object, values, employeeValues).Handle(null, default));
        }

        [Fact]
        public async void WhenAllCommandClassPropertyIsNull_ThrowBadRequestException()
        {
            await Assert.ThrowsAsync<BadRequestException>(async () => await new DeleteTerritoriesV1Handler(dbContext, userInfoMock.Object, values, employeeValues).Handle(new DeleteTerritoriesV1Command(), default));
        }

        [Fact]
        public async void WhenAllCommandClassPropertyIsEmpty_ThrowBadRequestException()
        {
            await Assert.ThrowsAsync<BadRequestException>(async () => await new DeleteTerritoriesV1Handler(dbContext, userInfoMock.Object, values, employeeValues).Handle(new DeleteTerritoriesV1Command() { TerritoryIds = new List<Guid>() }, default));
        }
        #endregion

        #region Scenarios Unit Tests
        [Fact]
        public async void WhenDeletingNonExistedTerritory_ThrowNotFoundException()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () => await new DeleteTerritoriesV1Handler(dbContext, userInfoMock.Object, values, employeeValues).Handle(new DeleteTerritoriesV1Command() { TerritoryIds = new List<Guid>() { Guid.NewGuid() } }, default));
        }

        [Fact(Skip = "This test isn't applying for ImMemory DB Tests.")]
        public async void WhenDeletingATerritoryWhichHasDatabaseDependances_ThrowDeleteFailureException()
        {
            var adminTerritory = dbContext.Territories.FirstOrDefault();

            await Assert.ThrowsAsync<DeleteFailureException>(async () => await new DeleteTerritoriesV1Handler(dbContext, userInfoMock.Object, values, employeeValues).Handle(new DeleteTerritoriesV1Command() { TerritoryIds = new List<Guid>() { adminTerritory.Id } }, default));
        }

        public async void AuthorizationScenariosUnitTest(string claimType, string claimValue, bool willThrowException)
        {

        }
        #endregion
    }
}
