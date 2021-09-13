using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Backend.Application.Common.Exceptions;
using WeeControl.Backend.Application.Common.Interfaces;
using WeeControl.Backend.Application.EntityGroups.Territory.Queries.GetTerritoryV1;
using WeeControl.Backend.Domain.Common.Interfaces;
using WeeControl.Backend.Domain.EntityGroups.Employee;
using WeeControl.Backend.Persistence;
using WeeControl.SharedKernel.DtosV1.Territory;
using WeeControl.SharedKernel.EntityGroups.Employee.Attributes;
using WeeControl.SharedKernel.EntityGroups.Employee.Enums;
using WeeControl.SharedKernel.EntityGroups.Employee.Interfaces;
using WeeControl.SharedKernel.EntityGroups.Territory.Attributes;
using WeeControl.SharedKernel.EntityGroups.Territory.Interfaces;
using Xunit;

namespace WeeControl.Backend.Application.Test.EntityGroup.Territory.V1.Queries
{
    public class GetTerritoriesHandlerTesters : IDisposable
    {
        private IMySystemDbContext dbContext;
        private Mock<ICurrentUserInfo> userInfoMock;
        private readonly ITerritoryAttribute values = new TerritoryAttribute();
        private readonly IEmployeeAttribute employeeValues = new EmployeeAttribute();

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
            userInfoMock.Setup(x => x.Territories).Returns(new List<Guid>() { territory.Id });
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
        public async void WhenGettingTerritoriesByTerritoryId_ReturnSingleOffice()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.Territories).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(employeeValues.GetClaimType(ClaimTypeEnum.HumanResources), employeeValues.GetClaimTag(ClaimTagEnum.Read)) });

            var territory = dbContext.Territories.FirstOrDefault();

            var responseDto = await new GetTerritoriesHandler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesQuery(territory.Id), default);

            Assert.Single(responseDto);
        }

        [Fact]
        public async void WhenGettingUnknownEmployeeTerritoriesByEmployeeId_ThrowNotFoundException()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.Territories).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(employeeValues.GetClaimType(ClaimTypeEnum.HumanResources), employeeValues.GetClaimTag(ClaimTagEnum.Read)) });

            await Assert.ThrowsAsync<NotFoundException>(async () => await new GetTerritoriesHandler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesQuery(Guid.NewGuid()), default));
        }

        [Fact]
        public async void WhenGettingUnknownEmployeeTerritoriesBySessionId_ThrowNotFoundException()
        {
            var admin = dbContext.Employees.FirstOrDefault();
            userInfoMock.Setup(x => x.Territories).Returns(new List<Guid>() { admin.TerritoryId });
            userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(employeeValues.GetClaimType(ClaimTypeEnum.HumanResources), employeeValues.GetClaimTag(ClaimTagEnum.Read)) });

            await Assert.ThrowsAsync<NotFoundException>(async () => await new GetTerritoriesHandler(dbContext, userInfoMock.Object, values).Handle(new GetTerritoriesQuery(Guid.NewGuid()), default));
        }
        #endregion
    }
}
