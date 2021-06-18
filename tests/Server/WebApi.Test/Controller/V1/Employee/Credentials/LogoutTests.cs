using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace WeeControl.Server.WebApi.Test.Controller.V1.Employee.Credentials
{
    public class LogoutTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public LogoutTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }
    }
}
