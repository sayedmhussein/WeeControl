using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Backend.Application.BoundContexts.Adminstration.Territory.Queries.GetTerritoryV1;
using WeeControl.Backend.Application.Interfaces;
using WeeControl.Backend.Persistence;
using WeeControl.Common.SharedKernel.EntityGroups.Employee.Attributes;
using WeeControl.Common.SharedKernel.EntityGroups.Employee.Interfaces;
using WeeControl.Common.SharedKernel.EntityGroups.Territory.Attributes;
using WeeControl.Common.SharedKernel.EntityGroups.Territory.Interfaces;
using Xunit;

namespace WeeControl.Backend.Application.Test.EntityGroup.Territory.V1.Queries
{
    public class GetTerritoriesHandlerTesters : IDisposable
    {
        private IMySystemDbContext dbContext;
        private Mock<ICurrentUserInfo> userInfoMock;
        private readonly ITerritoryAttribute values = new TerritoryAppSetting();
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
        // [Theory(Skip = "Authorization was execluded from this query")]
        // //[ClassData(typeof(GetTerritoriesHandlerTestersData))]
        // public async void AuthorizationScenariosUnitTest(string claimType, string claimValue, bool willThrowException)
        // {
        //     var territory = dbContext.Territories.FirstOrDefault();
        //     var user = new EmployeeDbo() { Territory = territory };
        //     await dbContext.Employees.AddAsync(user);
        //     await dbContext.SaveChangesAsync(default);
        //     //
        //     userInfoMock.Setup(x => x.Territories).Returns(new List<Guid>() { territory.Id });
        //     userInfoMock.Setup(x => x.Claims).Returns(new List<Claim>() { new Claim(claimType, claimValue) });
        //
        //     var query = new GetTerritoriesQuery(user.Id);
        //     var handler =  new GetTerritoriesHandler(dbContext, userInfoMock.Object, values);
        //
        //     if (willThrowException)
        //     {
        //         await Assert.ThrowsAsync<NotAllowedException>(async () => await handler.Handle(query, default));
        //     }
        //     else
        //     {
        //         var response = await handler.Handle(query, default);
        //
        //         Assert.IsType<List<TerritoryDto>>(response);
        //         Assert.NotEmpty(response);
        //     }
        // }
        #endregion
    }
}
