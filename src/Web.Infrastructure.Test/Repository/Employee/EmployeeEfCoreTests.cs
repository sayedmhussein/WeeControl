using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MySystem.Web.EfRepository;
using Web.Infrastructure.Repository.Employee;
using Xunit;
namespace Web.Infrastructure.Test.Repository.Employee
{
    public class EmployeeEfCoreTests
    {
        #region Constructor
        [Fact]
        public void WhenNullOptions_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EmployeeEfCore((DataContext)null));
        }
        #endregion

        #region GetSessionIdAsync
        [Theory]
        [ClassData(typeof(GetSessionIdAsyncTestData))]
        public async void GetSessionIdAsyncTheoryTests(
            DataContext context,
            string username, string password, string device,
            bool isNull,
            string clarification)
        {
            var repo = new EmployeeEfCore(context);

            var id = await repo.GetSessionIdAsync(username, password, device);

            Assert.NotEmpty(clarification);

            if (isNull)
                Assert.Null(id);
            else
                Assert.NotNull(id);

        }

        [Fact]
        public async void WhenValidUsernameAndPasswordCompination_ReturnUserId()
        {
            var repo = new EmployeeEfCore(ContextOptions.GetInMemoryOptions().Options);

            var id = await repo.GetSessionIdAsync("username", "password", "device");

            Assert.NotNull(id);
        }
        #endregion

        #region GetUserIdAsync
        [Fact]
        public async void WhenSessionExist_ReturnEmployeeId()
        {
            var repo = new EmployeeEfCore(ContextOptions.GetInMemoryOptions().Options);
            var sessionid = await repo.GetSessionIdAsync("username", "password", "device");

            var employeeid = await repo.GetUserIdAsync((Guid)sessionid);

            Assert.NotNull(employeeid);
        }

        [Fact]
        public async void WhenSessionNotExist_ReturnEmployeeId()
        {
            var repo = new EmployeeEfCore(ContextOptions.GetInMemoryOptions().Options);

            var employeeid = await repo.GetUserIdAsync(Guid.NewGuid());

            Assert.Null(employeeid);
        }
        #endregion

        #region TerminateSessionAsync
        [Fact]
        public async void WhenTerminatingExistingSession_UserIdShouldNotAppearAgainWhenQueringBySession()
        {
            var repo = new EmployeeEfCore(ContextOptions.GetInMemoryOptions().Options);
            var sessionid = await repo.GetSessionIdAsync("username", "password", "device");

            Assert.NotNull(await repo.GetUserIdAsync((Guid)sessionid));

            await repo.TerminateSessionAsync((Guid)sessionid);

            Assert.Null(await repo.GetUserIdAsync((Guid)sessionid));
        }

        [Fact]
        public async void WhenTerminatingInvalidSession_NothingHappens()
        {
            var repo = new EmployeeEfCore(ContextOptions.GetInMemoryOptions().Options);

            await repo.TerminateSessionAsync(Guid.NewGuid());
        }
        #endregion

        #region GetUserOfficesAsync
        [Fact]
        public async void WhenUserIsMainOffice_ReturnMoreThanOnOffice()
        {
            var repo = new EmployeeEfCore(ContextOptions.GetInMemoryOptions().Options);
            var sessionid = await repo.GetSessionIdAsync("username", "password", "device");
            var userid = await repo.GetUserIdAsync((Guid)sessionid);

            var offices = await repo.GetUserOfficesAsync((Guid)userid);

            Assert.True(offices.Count() > 2);
        }
        #endregion

        #region GetUserClaimsAsync
        [Fact]
        public async void WhenUserHasClaims_ReturnListNotEmpty()
        {
            var repo = new EmployeeEfCore(ContextOptions.GetInMemoryOptions().Options);
            var sessionid = await repo.GetSessionIdAsync("username", "password", "device");
            var userid = await repo.GetUserIdAsync((Guid)sessionid);

            var claims = await repo.GetUserClaimsAsync((Guid)userid);

            Assert.NotEmpty(claims);
        }
        #endregion
    }
}
