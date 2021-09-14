using System;
using System.Net.Http;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Backend.WebApi.Test.Functional.V1.Territory;
using WeeControl.SharedKernel.Routing;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.V1
{
    public class FunctionalTestTemplate : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly IFunctionalTest test;
        private readonly IFunctionalAuthorization authorization;
        private readonly Uri routeUri;
        
        public FunctionalTestTemplate(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            test = new FunctionalTest(factory, HttpMethod.Get, typeof(HttpGetTests).Namespace);
            authorization = new FunctionalAuthorization(test);
            routeUri = test.GetUri(ApiRouteEnum.Territory);
        }
    }
}
