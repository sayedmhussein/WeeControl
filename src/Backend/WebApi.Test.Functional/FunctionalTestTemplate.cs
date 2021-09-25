using System;
using System.Net.Http;
using Moq;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.Interfaces;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional
{
    public class FunctionalTestTemplate : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly string device;
        private Mock<IClientDevice> clientDeviceMock;
        
        public FunctionalTestTemplate(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            device = nameof(FunctionalTestTemplate);
            
            clientDeviceMock = new Mock<IClientDevice>();
            clientDeviceMock.SetupAllProperties();
            clientDeviceMock.Setup(x => x.DeviceId).Returns(device);
        }
        
        public void Dispose()
        {
            clientDeviceMock = null;
        }
    }
}
