using System;
using System.Net.Http;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel.Routing;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional
{
    public class FunctionalTestTemplate : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly IFunctionalTest test;
        private readonly Uri routeUri;
        
        public FunctionalTestTemplate(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            test = new FunctionalTest(factory, typeof(FunctionalTestTemplate).Namespace, HttpMethod.Get, "1.0");
            routeUri = test.GetUri(ApiRouteEnum.Territory);
        }
        
        
    }
}
