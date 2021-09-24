using System;
using System.Net.Http;
using WeeControl.Backend.WebApi.Test.Functional.BoundedContexts.HumanResources.Authorization;
using WeeControl.Backend.WebApi.Test.Functional.TestHelpers;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.BoundedContextDtos.HumanResources.Authorization;
using WeeControl.Common.SharedKernel.Obsolute.Common;
using WeeControl.Common.SharedKernel.Routing;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional
{
    public class FunctionalTestTemplate : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> factory;
        private readonly IFunctionalTestService testService;
        private readonly string device;
        
        public FunctionalTestTemplate(CustomWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            device = nameof(FunctionalTestTemplate);
            
            HttpRequestMessage defaultRequestMessage = new()
            {
                RequestUri = new Uri(ApiRouteLink.HumanResources.Authorization.RequestNewToken.Absolute),
                Version = new Version(ApiRouteLink.HumanResources.Authorization.RequestNewToken.Version),
                Method = ApiRouteLink.HumanResources.Authorization.RequestNewToken.Method,
                Content = FunctionalTestService.GetHttpContentAsJson(new RequestDto<object>(null, device))
            };
            
            testService = new FunctionalTestService(factory);
        }
        
        
    }
}
