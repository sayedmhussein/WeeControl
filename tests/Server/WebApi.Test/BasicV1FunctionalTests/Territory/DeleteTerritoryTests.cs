using System;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
using Xunit;

namespace WeeControl.Server.WebApi.Test.BasicV1FunctionalTests.Territory
{
    public class DeleteTerritoryTests : BaseFunctionalTest, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly Uri RequstUri;
        private readonly WebApplicationFactory<Startup> factory;

        public DeleteTerritoryTests(WebApplicationFactory<Startup> factory)
            : base(factory.CreateClient(), EmployeeName.Admin)
        {
            RequstUri = new Uri(BaseUri, ApiRoute[ApiRouteEnum.Territory]);
            this.factory = factory;
        }

        [Fact]
        public async void WhenDeletingTerritoryWithoutToken_ReturnUnauthorizedHttpCodeRespose()
        {
            var client = factory.CreateClient();

            var response = await client.DeleteAsync(RequstUri + Guid.Empty.ToString());

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
