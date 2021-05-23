using System;
using Web.Infrastructure.Repository.EfCore;
using Xunit;
namespace Web.Infrastructure.Test.Repository.EfCore
{
    public class UserRepositoryTest
    {
        #region Constructor
        [Fact]
        public void WhenNullOptions_ThrowNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => new UserRepository(null));
        }
        #endregion

        #region GetUserId
        [Fact]
        public async void WhenValidUsernameAndPasswordCompination_ReturnUserId()
        {
            var repo = new UserRepository(EfCoreOptions.GetInMemoryOptions());

            var id = await repo.GetUserId("username", "password");

            Assert.NotNull(id);
        }

        [Fact]
        public async void WhenInValidUsernameAndPasswordCompination_ReturnNullId()
        {
            var repo = new UserRepository(EfCoreOptions.GetInMemoryOptions());

            var id = await repo.GetUserId("username1", "password");

            Assert.Null(id);
        }
        #endregion

        #region CreateOrGetSession
        [Fact]
        public async void WhenValidUserName_ReturnNewSessionId()
        {
            var repo = new UserRepository(EfCoreOptions.GetInMemoryOptions());

            var session = await repo.CreateOrGetSessionAsync("username", "device");

            Assert.NotNull(session);
            Assert.IsType<Guid>(session);
        }

        [Fact]
        public async void WhenInValidUserName_ReturnNullSession()
        {
            var repo = new UserRepository(EfCoreOptions.GetInMemoryOptions());

            var session = await repo.CreateOrGetSessionAsync("username1", "device");

            Assert.Null(session);
        }
        #endregion

        #region TerminateSessionAsync
        [Fact]
        public async void WhenValidSessionId_ReturnTrue()
        {
            var repo = new UserRepository(EfCoreOptions.GetInMemoryOptions());
            var id = await repo.CreateOrGetSessionAsync("username", "device");

            var exist = await repo.TerminateSessionAsync((Guid)id);

            Assert.True(exist);
        }

        [Fact]
        public async void WhenInValidSessionId_ReturnFalse()
        {
            var repo = new UserRepository(EfCoreOptions.GetInMemoryOptions());

            var exist = await repo.TerminateSessionAsync(Guid.NewGuid());

            Assert.False(exist);
        }
        #endregion

        #region LogToSessionAsync & GetSessionLog
        [Fact]
        public async void WhenLoggingToSession_()
        {
            var repo = new UserRepository(EfCoreOptions.GetInMemoryOptions());
            Guid id = (Guid)await repo.CreateOrGetSessionAsync("username", "device");

            string arg = "Some Text";
            await repo.LogToSessionAsync(id, arg);

            //var list = await repo.GetSessionLogAsync(id);
            //Assert.Contains(list.FirstOrDefault().arg, arg);
        }
        #endregion
    }
}
