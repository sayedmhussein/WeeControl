using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Sayed.MySystem.ClientService.Services;
using Sayed.MySystem.Shared.Configuration.Models;
using Xunit;

namespace Sayed.MySystem.ClientService.UnitTest.Services
{
    public class ClientServicesLoggingTesters
    {
        [Fact]
        public void WhenLoggingSomething_ShouldGetSameLogTextSaved()
        {
            string file = "_test_log.log";
            string str = "This is Log";
            var service = new ClientServices(null, new Mock<IApi>().Object);

            service.LogAppend(str, file);

            Assert.Contains(str, service.LogReadAll(file));
        }

        [Fact]
        public async void WhenConcurrentLogginProcessInAction_ShouldNotThrowException()
        {
            string file = "_test2_log.log";
            string str = "This is Log";
            var service = new ClientServices(null, new Mock<IApi>().Object);
            var tasks = new List<Task>(); ;

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() => service.LogAppend(str, file)));
            }

            await Task.WhenAll(tasks);

            Assert.NotEmpty(service.LogReadAll(file));
        }
    }
}
