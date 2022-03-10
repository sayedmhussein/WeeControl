using System;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.BoundedContexts.Credentials
{
    public class ReginsterTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly CustomWebApplicationFactory<Startup> factory;

        public ReginsterTests(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
