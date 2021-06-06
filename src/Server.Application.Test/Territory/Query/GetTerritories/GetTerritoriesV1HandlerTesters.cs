using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MySystem.Application.Common.Exceptions;
using MySystem.Application.Common.Interfaces;
using MySystem.Application.Territory.Query.GetTerritories;
using MySystem.Domain.EntityDbo.EmployeeSchema;
using MySystem.Domain.EntityDbo.Territory;
using MySystem.Persistence;
using MySystem.SharedKernel.EntityV1Dtos.Territory;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.Services;
using Xunit;

namespace MySystem.Application.Test.Territory.Query.GetTerritories
{
    public class GetTerritoriesV1HandlerTesters : IDisposable
    {
        private IMySystemDbContext dbContext;
        private Mock<ICurrentUserInfo> userInfoMock;
        private IValuesService values = new ValueService();

        public GetTerritoriesV1HandlerTesters()
        {
            dbContext = new ServiceCollection().AddPersistenceAsInMemory(new Random().NextDouble().ToString()).BuildServiceProvider().GetService<IMySystemDbContext>();
            userInfoMock = new Mock<ICurrentUserInfo>();
            
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

            Assert.Throws<ArgumentNullException>(() => new GetTerritoriesV1Handler(null, userInfoMock.Object, values));
        }

        [Fact]
        public void WhenUserInfoIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new GetTerritoriesV1Handler(dbContext, null, values));
        }
        #endregion

        #region Handle Parameters Unit Tests
        [Fact]
        public async void WhenQueryIsNull_ThrowArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await new GetTerritoriesV1Handler(dbContext, userInfoMock.Object, values).Handle(null, default));
        }

        [Fact]
        public async void WhenAllQueryParametersAreNull_ReturnCurrentUserTerritories()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });

            var list = await new GetTerritoriesV1Handler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesV1Query(), default);

            Assert.NotEmpty(list);
        }
        #endregion

        #region Scenarios Unit Tests
        [Theory]
        [ClassData(typeof(AuthorizationScenariosTestData))]
        public async void AuthorizationScenariosUnitTest(string claimType, string claimValue, bool willThrowException)
        {
            var territory = dbContext.Territories.FirstOrDefault();
            var user = new EmployeeDbo() { Territory = territory };
            await dbContext.Employees.AddAsync(user);
            await dbContext.SaveChangesAsync(default);
            //
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { territory.Id });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(claimType, claimValue) });

            var query = new GetTerritoriesV1Query() { EmployeeId = user.Id };
            var handler =  new GetTerritoriesV1Handler(dbContext, userInfoMock.Object, values);

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
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(values.ClaimType[ClaimTypeEnum.HumanResources], values.ClaimTag[ClaimTagEnum.Read]) });

            var responseDto = await new GetTerritoriesV1Handler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesV1Query() { EmployeeId = admin.Id }, default);

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
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(values.ClaimType[ClaimTypeEnum.HumanResources], values.ClaimTag[ClaimTagEnum.Read]) });

            var responseDto = await new GetTerritoriesV1Handler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesV1Query() { SessionId = session.Id }, default);

            Assert.Single(responseDto);
        }

        [Fact(Skip = "This feature not implemented yet!")]
        public async void WhenGettingTerritoriesByTerritoryId_ReturnSingleOffice()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(values.ClaimType[ClaimTypeEnum.HumanResources], values.ClaimTag[ClaimTagEnum.Read]) });

            var territory = dbContext.Territories.FirstOrDefault();

            var responseDto = await new GetTerritoriesV1Handler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesV1Query() { TerritoryId = territory.Id }, default);

            Assert.Single(responseDto);
        }

        [Fact]
        public async void WhenGettingUnknownEmployeeTerritoriesByEmployeeId_ThrowNotFoundException()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(values.ClaimType[ClaimTypeEnum.HumanResources], values.ClaimTag[ClaimTagEnum.Read]) });

            await Assert.ThrowsAsync<NotFoundException>(async () => await new GetTerritoriesV1Handler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesV1Query() { EmployeeId = Guid.NewGuid() }, default));
        }

        [Fact]
        public async void WhenGettingUnknownEmployeeTerritoriesBySessionId_ThrowNotFoundException()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(values.ClaimType[ClaimTypeEnum.HumanResources], values.ClaimTag[ClaimTagEnum.Read]) });

            await Assert.ThrowsAsync<NotFoundException>(async () => await new GetTerritoriesV1Handler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesV1Query() { SessionId = Guid.NewGuid() }, default));
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesByEmployeeIdWhichHasOneMoreTerritory_ReturnDoubleTerritories()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(values.ClaimType[ClaimTypeEnum.HumanResources], values.ClaimTag[ClaimTagEnum.Read]) });

            var territory = dbContext.Territories.FirstOrDefault();
            await dbContext.Territories.AddAsync(new TerritoryDbo() { ReportTo = territory, CountryId = "sss", Name = "name" });
            await dbContext.SaveChangesAsync(default);

            var responseDto = await new GetTerritoriesV1Handler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesV1Query() { EmployeeId = admin.Id }, default);

            Assert.Equal(2, responseDto.Count());
        }

        [Fact]
        public async void WhenGettingAdminTerritoriesByEmployeeIdWhichHasTwoMoreTerritoryOneLevelDown_ReturnTripleTerritories()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(values.ClaimType[ClaimTypeEnum.HumanResources], values.ClaimTag[ClaimTagEnum.Read]) });

            var territory = dbContext.Territories.FirstOrDefault();
            await dbContext.Territories.AddAsync(new TerritoryDbo() { ReportTo = territory, CountryId = "sss", Name = "name1" });
            await dbContext.Territories.AddAsync(new TerritoryDbo() { ReportTo = territory, CountryId = "sss", Name = "name2" });
            await dbContext.SaveChangesAsync(default);

            var responseDto = await new GetTerritoriesV1Handler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesV1Query() { EmployeeId = admin.Id }, default);

            Assert.Equal(3, responseDto.Count());
        }

        [Fact(Skip = "This test produce a known bug, it will be solved soon.")]
        public async void WhenGettingAdminTerritoriesByEmployeeIdWhichHasTwoMoreTerritoryTwoLevelsDown_ReturnTripleTerritories()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.TerritoriesId).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(values.ClaimType[ClaimTypeEnum.HumanResources], values.ClaimTag[ClaimTagEnum.Read]) });

            var territory1 = dbContext.Territories.FirstOrDefault();
            //
            var territory2 = new TerritoryDbo() { ReportTo = territory1, CountryId = "sss", Name = "name1" };
            await dbContext.Territories.AddAsync(territory2);
            await dbContext.SaveChangesAsync(default);
            //
            var territory3 = new TerritoryDbo() { ReportTo = territory2, CountryId = "sss", Name = "name1" };
            await dbContext.Territories.AddAsync(territory3);
            await dbContext.SaveChangesAsync(default);

            var responseDto = await new GetTerritoriesV1Handler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesV1Query() { EmployeeId = admin.Id }, default);

            Assert.Equal(3, responseDto.Count());
        }
        #endregion
    }
}
