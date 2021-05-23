using System;
using Xunit;
using Moq;
using MySystem.Web.Api.Domain.Employee;

namespace Web.Domain.Test.User
{
    public class UserServiceTest
    {
        #region Constructor
        [Fact]
        public void WhenPassingNullUserRepository_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EmployeeService(null));
        }
        #endregion

        #region GetUserSessionAsync
        [Fact]
        public void WhenNullUsername_ThrowArgumentNullException()
        {
            var repoMock = new Mock<IEmployeeRepository>();

            var service = new UserService(repoMock.Object);

            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.GetUserSessionAsync(null, "password", "device"));
        }

        [Fact]
        public void WhenNullPassword_ThrowArgumentNullException()
        {
            var repoMock = new Mock<IEmployeeRepository>();

            var service = new UserService(repoMock.Object);

            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.GetUserSessionAsync("username", null, "device"));
        }

        [Fact]
        public async void WhenValidUsernameAndPassword_ReturnSessionId()
        {
            var repoMock = new Mock<IEmployeeRepository>();
            //repoMock.Setup(x => x.);

            var service = new UserService(repoMock.Object);
            var id = await service.GetUserSessionAsync("username", "password", "device");

            Assert.NotNull(id);
        }
        #endregion
    }
}
