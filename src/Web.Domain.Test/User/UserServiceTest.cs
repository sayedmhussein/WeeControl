using System;
using Xunit;
using Moq;
using MySystem.Web.Api.Domain.User;

namespace Web.Domain.Test.User
{
    public class UserServiceTest
    {
        #region Constructor
        [Fact]
        public void WhenPassingNullUserRepository_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new UserService(null));
        }
        #endregion
    }
}
