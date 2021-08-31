﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Backend.Application.Common.Exceptions;
using WeeControl.Backend.Application.Common.Interfaces;
using WeeControl.Backend.Application.Territory.Queries.GetTerritoryV1;
using WeeControl.Backend.Domain.BasicDbos.EmployeeSchema;
using WeeControl.Backend.Domain.BasicDbos.Territory;
using WeeControl.Backend.Domain.Interfaces;
using WeeControl.Backend.Persistence;
using WeeControl.SharedKernel.Aggregates.Employee;
using WeeControl.SharedKernel.Aggregates.Employee.Enums;
using WeeControl.SharedKernel.Aggregates.Territory;
using WeeControl.SharedKernel.Aggregates.Territory.DtosV1;
using Xunit;

namespace WeeControl.Backend.Application.Test.Territory.V1.Queries
{
    public class GetTerritoriesHandlerTesters : IDisposable
    {
        private IMySystemDbContext dbContext;
        private Mock<ICurrentUserInfo> userInfoMock;
        private readonly ITerritoryLists values = new TerritoryLists();
        private readonly IEmployeeLists employeeValues = new EmployeeLists();

        public GetTerritoriesHandlerTesters()
        {
            dbContext = new ServiceCollection().AddPersistenceAsInMemory(new Random().NextDouble().ToString()).BuildServiceProvider().GetService<IMySystemDbContext>();
            userInfoMock = new Mock<ICurrentUserInfo>();
            
        }

        public void Dispose()
        {
            dbContext = null;
            userInfoMock = null;
        }

        #region Constructor
        [Fact]
        public void WhenDbContextIsNull_ThrowsArgumentNullException()
        {
            userInfoMock = new Mock<ICurrentUserInfo>();

            Assert.Throws<ArgumentNullException>(() => new GetTerritoriesHandler(null, userInfoMock.Object, values));
        }

        [Fact]
        public void WhenUserInfoIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new GetTerritoriesHandler(dbContext, null, values));
        }

        [Fact]
        public void WhenSharedValueIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new GetTerritoriesHandler(dbContext, userInfoMock.Object, null));
        }
        #endregion

        #region Query
        [Fact]
        public async void WhenQueryIsNull_ThrowArgumentNullException()
        {
            var handler = new GetTerritoriesHandler(dbContext, userInfoMock.Object, values);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await handler.Handle(null, default));
        }

        //[Fact]
        //public async void WhenAllQueryParametersAreNull_ReturnCurrentUserTerritories()
        //{
        //    var admin = dbContext.Employees.FirstOrDefault();
        //    userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });

        //    var list = await new GetTerritoriesV1Handler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesV1Query(), default);

        //    Assert.NotEmpty(list);
        //    Assert.Contains(admin.TerritoryId, list.Select(x => x.Id));
        //}

        [Fact]
        public async void WhenSessionIdIsNotNullWhileOthersAreNull_ReturnListIgnoringSafetyRules()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            var session = new EmployeeSessionDbo() { Employee = admin, DeviceId = "device" };
            await dbContext.EmployeeSessions.AddAsync(session);
            await dbContext.SaveChangesAsync(default);

            var responseDto = await new GetTerritoriesHandler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesQuery(session.Id), default);

            Assert.Single(responseDto);
        }
        #endregion

        #region Scenarios Unit Tests
        [Theory(Skip = "Authorization was execluded from this query")]
        [ClassData(typeof(GetTerritoriesHandlerTestersData))]
        public async void AuthorizationScenariosUnitTest(string claimType, string claimValue, bool willThrowException)
        {
            var territory = dbContext.Territories.FirstOrDefault();
            var user = new EmployeeDbo() { Territory = territory };
            await dbContext.Employees.AddAsync(user);
            await dbContext.SaveChangesAsync(default);
            //
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { territory.Id });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(claimType, claimValue) });

            var query = new GetTerritoriesQuery(user.Id);
            var handler =  new GetTerritoriesHandler(dbContext, userInfoMock.Object, values);

            if (willThrowException)
            {
                await Assert.ThrowsAsync<NotAllowedException>(async () => await handler.Handle(query, default));
            }
            else
            {
                var response = await handler.Handle(query, default);

                Assert.IsType<List<TerritoryDto>>(response);
                Assert.NotEmpty(response);
            }
        }
        #endregion

        #region Legacy Unit Tests
        [Fact]
        public async void WhenGettingAdminTerritoriesByEmployeeId_ReturnSingleOffice()
        {
            var admin = dbContext.Employees.FirstOrDefault();

            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(employeeValues.GetClaimType(ClaimTypeEnum.HumanResources), employeeValues.GetClaimTag(ClaimTagEnum.Read)) });

            var responseDto = await new GetTerritoriesHandler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesQuery(admin.Id), default);

            Assert.Single(responseDto);
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesBySessionId_ReturnSingleOffice()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            var session = new EmployeeSessionDbo() { Employee = admin, DeviceId = "device" };
            await dbContext.EmployeeSessions.AddAsync(session);
            await dbContext.SaveChangesAsync(default);

            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(employeeValues.GetClaimType(ClaimTypeEnum.HumanResources), employeeValues.GetClaimTag(ClaimTagEnum.Read)) });

            var responseDto = await new GetTerritoriesHandler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesQuery(session.Id), default);

            Assert.Single(responseDto);
        }

        [Fact]
        public async void WhenGettingTerritoriesByTerritoryId_ReturnSingleOffice()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(employeeValues.GetClaimType(ClaimTypeEnum.HumanResources), employeeValues.GetClaimTag(ClaimTagEnum.Read)) });

            var territory = dbContext.Territories.FirstOrDefault();

            var responseDto = await new GetTerritoriesHandler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesQuery(territory.Id), default);

            Assert.Single(responseDto);
        }

        [Fact]
        public async void WhenGettingUnknownEmployeeTerritoriesByEmployeeId_ThrowNotFoundException()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(employeeValues.GetClaimType(ClaimTypeEnum.HumanResources), employeeValues.GetClaimTag(ClaimTagEnum.Read)) });

            await Assert.ThrowsAsync<NotFoundException>(async () => await new GetTerritoriesHandler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesQuery(Guid.NewGuid()), default));
        }

        [Fact]
        public async void WhenGettingUnknownEmployeeTerritoriesBySessionId_ThrowNotFoundException()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(employeeValues.GetClaimType(ClaimTypeEnum.HumanResources), employeeValues.GetClaimTag(ClaimTagEnum.Read)) });

            await Assert.ThrowsAsync<NotFoundException>(async () => await new GetTerritoriesHandler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesQuery(Guid.NewGuid()), default));
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesByEmployeeIdWhichHasOneMoreTerritory_ReturnDoubleTerritories()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(employeeValues.GetClaimType(ClaimTypeEnum.HumanResources), employeeValues.GetClaimTag(ClaimTagEnum.Read)) });

            var territory = dbContext.Territories.FirstOrDefault();
            await dbContext.Territories.AddAsync(new TerritoryDbo() { ReportTo = territory, CountryId = "sss", Name = "name" });
            await dbContext.SaveChangesAsync(default);

            var responseDto = await new GetTerritoriesHandler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesQuery(admin.Id), default);

            Assert.Equal(2, responseDto.Count());
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesByEmployeeIdWhichHasTwoMoreTerritoryOneLevelDown_ReturnTripleTerritories()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(employeeValues.GetClaimType(ClaimTypeEnum.HumanResources), employeeValues.GetClaimTag(ClaimTagEnum.Read)) });

            var territory = dbContext.Territories.FirstOrDefault();
            await dbContext.Territories.AddAsync(new TerritoryDbo() { ReportTo = territory, CountryId = "sss", Name = "name1" });
            await dbContext.Territories.AddAsync(new TerritoryDbo() { ReportTo = territory, CountryId = "sss", Name = "name2" });
            await dbContext.SaveChangesAsync(default);

            var responseDto = await new GetTerritoriesHandler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesQuery(admin.Id), default);

            Assert.Equal(3, responseDto.Count());
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesByEmployeeIdWhichHasTwoMoreTerritoryTwoLevelsDown_ReturnTripleTerritories()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(employeeValues.GetClaimType(ClaimTypeEnum.HumanResources), employeeValues.GetClaimTag(ClaimTagEnum.Read)) });

            var territory1 = dbContext.Territories.FirstOrDefault();
            //
            var territory2 = new TerritoryDbo() { ReportTo = territory1, CountryId = "sss", Name = "name1" };
            await dbContext.Territories.AddAsync(territory2);
            await dbContext.SaveChangesAsync(default);
            //
            var territory3 = new TerritoryDbo() { ReportTo = territory2, CountryId = "sss", Name = "name1" };
            await dbContext.Territories.AddAsync(territory3);
            await dbContext.SaveChangesAsync(default);

            var responseDto = await new GetTerritoriesHandler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesQuery(admin.Id), default);

            Assert.Equal(3, responseDto.Count());
        }
        #endregion
    }
}