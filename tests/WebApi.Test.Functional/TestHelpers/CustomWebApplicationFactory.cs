using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using WeeControl.Backend.Persistence;
using WeeControl.Common.FunctionalService.Interfaces;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = true, MaxParallelThreads = 1)]

namespace WeeControl.Backend.WebApi.Test.Functional.TestHelpers
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => { services.AddPersistenceAsInMemory(); });
        }

        public Mock<IUserDevice> GetUserDeviceMock(string device)
        {
            var mock = new Mock<IUserDevice>();
            mock.SetupAllProperties();
            mock.Setup(x => x.DeviceId).Returns(device);
            mock.Setup(x => x.TimeStamp).Returns(DateTime.UtcNow);
            return mock;
        }

        public Mock<IUserCommunication> GetUserCommunicationMock(HttpClient httpClient)
        {
            var mock = new Mock<IUserCommunication>();
            mock.SetupAllProperties();
            mock.Setup(x => x.ServerBaseAddress).Returns(GetLocalIpAddress());
            mock.Setup(x => x.HttpClient).Returns(httpClient);
            return mock;
        }

        public Mock<IUserStorage> GetUserStorageMockMock()
        {
            var mock = new Mock<IUserStorage>();
            mock.SetupAllProperties();
            return mock;
        }
        
        private static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return "http://" + ip.ToString() + ":5000/";
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
