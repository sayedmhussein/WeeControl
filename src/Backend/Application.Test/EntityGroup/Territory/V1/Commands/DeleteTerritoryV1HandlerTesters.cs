using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Backend.Application.BoundContexts.Adminstration.Territory.Commands.DeleteTerritoryV1;
using WeeControl.Backend.Application.Exceptions;
using WeeControl.Backend.Application.Interfaces;
using WeeControl.Backend.Persistence;
using WeeControl.Common.SharedKernel.EntityGroups.Employee.Attributes;
using WeeControl.Common.SharedKernel.EntityGroups.Employee.Interfaces;
using WeeControl.Common.SharedKernel.EntityGroups.Territory.Attributes;
using WeeControl.Common.SharedKernel.EntityGroups.Territory.Interfaces;
using Xunit;

namespace WeeControl.Backend.Application.Test.EntityGroup.Territory.V1.Commands
{
    public class DeleteTerritoryV1HandlerTesters : IDisposable
    {
        private IMySystemDbContext dbContext;
        private Mock<ICurrentUserInfo> userInfoMock;
        private readonly IEmployeeAttribute employeeValues = new EmployeeAttribute();
        private readonly ITerritoryAttribute values = new TerritoryAppSetting();

        public DeleteTerritoryV1HandlerTesters()
        {
            dbContext = new ServiceCollection().AddPersistenceAsInMemory(new Random().NextDouble().ToString()).BuildServiceProvider().GetService<IMySystemDbContext>();

            userInfoMock = new Mock<ICurrentUserInfo>();
            userInfoMock.Setup(x => x.Territories).Returns(new List<Guid>() { dbContext.Employees.FirstOrDefault().Id });
            // userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(employeeValues.GetClaimType(ClaimTypeEnum.HumanResources), employeeValues.GetClaimTag(ClaimTagEnum.Delete)) });
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

            Assert.Throws<ArgumentNullException>(() => new DeleteTerritoryHandler(null, userInfoMock.Object, values, employeeValues));
        }

        [Fact]
        public void WhenUserInfoIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteTerritoryHandler(dbContext, null, values, employeeValues));
        }
        #endregion

        #region Handle Parameters Unit Tests
        [Fact]
        public async void WhenCommandIsNull_ThrowArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await new DeleteTerritoryHandler(dbContext, userInfoMock.Object, values, employeeValues).Handle(null, default));
        }
        #endregion

        #region Scenarios Unit Tests
        [Fact]
        public async void WhenDeletingNonExistedTerritory_ThrowNotFoundException()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () => await new DeleteTerritoryHandler(dbContext, userInfoMock.Object, values, employeeValues).Handle(new DeleteTerritoryCommand(Guid.NewGuid()), default));
        }

        [Fact]
        public async void WhenDeletingATerritoryWhichHasDatabaseDependances_ThrowDeleteFailureException()
        {
            var adminTerritory = dbContext.Territories.FirstOrDefault();
            await dbContext.Territories.AddAsync(new TerritoryDbo()
            {
                CountryId = "TST",
                Name = new Random().NextDouble().ToString(),
                ReportToId = adminTerritory.Id
            });
            await dbContext.SaveChangesAsync(default);

            await Assert.ThrowsAsync<DeleteFailureException>(async () => await new DeleteTerritoryHandler(dbContext, userInfoMock.Object, values, employeeValues).Handle(new DeleteTerritoryCommand(adminTerritory.Id), default));
        }
        #endregion
    }
}
