using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Domain.BasicDbos.EmployeeSchema;
using WeeControl.Backend.Domain.Interfaces;
using WeeControl.Backend.Persistence;
using WeeControl.SharedKernel.Common;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Controllers.Territory.V1
{
    public class HttpDeleteTests :
        IClassFixture<WebApplicationFactory<Startup>>,
        IDisposable
    {
        private readonly IHttpMessage httpMessage;
        private readonly IFunctionalAuthorization userToken;
        private HttpRequestMessage request;

        public HttpDeleteTests(WebApplicationFactory<Startup> factory)
        {
            factory.WithWebHostBuilder(c => c.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<MySystemDbContext>));

                services.Remove(descriptor);

                services.AddPersistenceAsInMemory("DbName");

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MySystemDbContext>();

                db.Database.EnsureCreated();

                db.EmployeeSessions.Add(new EmployeeSessionDbo() { });
            }));

            var client = factory.CreateClient();

  

            httpMessage = new HttpMessage(client, typeof(FunctionalTestTemplate).Namespace);
            userToken = new FunctionalAuthorization(httpMessage);

            request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                Version = new Version("1.0"),
                RequestUri = httpMessage.GetUri(ApiRouteEnum.Territory)
            };
        }

        public void Dispose()
        {
            request = null;
        }

        [Fact]
        public async void WhenWithoutToken_ReturnUnAuthorized()
        {
            var response = await httpMessage.GetResponseMessageAsync(request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
