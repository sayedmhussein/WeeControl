using System;
using System.Net.Http;
using WeeControl.Backend.WebApi.Test.Functional.V1.Territory;
using WeeControl.SharedKernel.Helpers;
using Xunit;

namespace WeeControl.Backend.WebApi.Test.Functional.V1
{
    public class ExampleTest :
        BaseFunctionalTest,
        IClassFixture<CustomWebApplicationFactory<Startup>>,
        IDisposable
    {
        public ExampleTest(CustomWebApplicationFactory<Startup> factory) :
            base(factory, HttpMethod.Delete, typeof(HttpGetTests).Namespace)
        {
            ServerUri = GetUri(ApiRouteEnum.Territory);
        }
    }
}
