﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Server.Application.Common.Exceptions;
using WeeControl.Server.Application.Common.Interfaces;
using WeeControl.Server.Application.Territory.Commands.DeleteTerritories;
using WeeControl.Server.Application.Territory.V1.Commands;
using WeeControl.Server.Domain.Interfaces;
using WeeControl.Server.Persistence;
using WeeControl.SharedKernel.BasicSchemas.Employee;
using WeeControl.SharedKernel.BasicSchemas.Employee.Enums;
using WeeControl.SharedKernel.BasicSchemas.Territory;
using Xunit;

namespace WeeControl.Server.Application.Test.Territory.V1.Commands
{
    public class DeleteTerritoryV1HandlerTesters : IDisposable
    {
        private IMySystemDbContext dbContext;
        private Mock<ICurrentUserInfo> userInfoMock;
        private readonly IEmployeeLists employeeValues = new EmployeeLists();
        private readonly ITerritoryLists values = new TerritoryLists();

        public DeleteTerritoryV1HandlerTesters()
        {
            dbContext = new ServiceCollection().AddPersistenceAsInMemory(new Random().NextDouble().ToString()).BuildServiceProvider().GetService<IMySystemDbContext>();

            userInfoMock = new Mock<ICurrentUserInfo>();
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { dbContext.Employees.FirstOrDefault().Id });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(employeeValues.GetClaimType(ClaimTypeEnum.HumanResources), employeeValues.GetClaimTag(ClaimTagEnum.Delete)) });
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

            Assert.Throws<ArgumentNullException>(() => new DeleteTerritoriesHandler(null, userInfoMock.Object, values, employeeValues));
        }

        [Fact]
        public void WhenUserInfoIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteTerritoriesHandler(dbContext, null, values, employeeValues));
        }
        #endregion

        #region Handle Parameters Unit Tests
        [Fact]
        public async void WhenCommandIsNull_ThrowArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await new DeleteTerritoriesHandler(dbContext, userInfoMock.Object, values, employeeValues).Handle(null, default));
        }

        [Fact]
        public async void WhenAllCommandClassPropertyIsNull_ThrowBadRequestException()
        {
            await Assert.ThrowsAsync<BadRequestException>(async () => await new DeleteTerritoriesHandler(dbContext, userInfoMock.Object, values, employeeValues).Handle(new DeleteTerritoriesCommand(), default));
        }

        [Fact]
        public async void WhenAllCommandClassPropertyIsEmpty_ThrowBadRequestException()
        {
            await Assert.ThrowsAsync<BadRequestException>(async () => await new DeleteTerritoriesHandler(dbContext, userInfoMock.Object, values, employeeValues).Handle(new DeleteTerritoriesCommand() { TerritoryIds = new List<Guid>() }, default));
        }
        #endregion

        #region Scenarios Unit Tests
        [Fact]
        public async void WhenDeletingNonExistedTerritory_ThrowNotFoundException()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () => await new DeleteTerritoriesHandler(dbContext, userInfoMock.Object, values, employeeValues).Handle(new DeleteTerritoriesCommand() { TerritoryIds = new List<Guid>() { Guid.NewGuid() } }, default));
        }

        [Fact(Skip = "This test isn't applying for ImMemory DB Tests.")]
        public async void WhenDeletingATerritoryWhichHasDatabaseDependances_ThrowDeleteFailureException()
        {
            var adminTerritory = dbContext.Territories.FirstOrDefault();

            await Assert.ThrowsAsync<DeleteFailureException>(async () => await new DeleteTerritoriesHandler(dbContext, userInfoMock.Object, values, employeeValues).Handle(new DeleteTerritoriesCommand() { TerritoryIds = new List<Guid>() { adminTerritory.Id } }, default));
        }
        #endregion
    }
}