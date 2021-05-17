using System;
using Sayed.MySystem.ClientService.ViewModels;
using Xunit;
using Moq;
using Sayed.MySystem.ClientService.Services;
using System.Threading.Tasks;
using Sayed.MySystem.Shared.Configuration.Models;
using Microsoft.Extensions.Logging;

namespace Sayed.MySystem.ClientService.UnitTest.ViewModels
{
    public class LoginViewModelTesters
    {
        [Fact]
        public async void WhenEmptyUsername_ThrowInvalidArgumentException()
        {
            var device = new Mock<IDevice>();
            device.Setup(x => x.Token).Returns(new Random().NextDouble().ToString());
            device.Setup(x => x.Internet).Returns(true);
            //
            var service = new ClientServices(new Mock<IDevice>().Object, new Mock<IApi>().Object, new Mock<ILogger>().Object);

            var vm = new LoginViewModel(device.Object, service)
            {
                Username = string.Empty,
                Password = "SomePassword"
            };

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await vm.LoginCommand.ExecuteAsync(null));
        }
    }
}
