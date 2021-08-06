//using System;
//using System.Net.Http;
//using Microsoft.AspNetCore.Mvc.Testing;
//using WeeControl.SharedKernel.Common;
//using Xunit;

//namespace WeeControl.Server.WebApi.Test.Controllers
//{
//    public class FunctionalTestTemplate :
//        BaseFunctionalTest,
//        IClassFixture<WebApplicationFactory<Startup>>,
//        IDisposable
//    {
//        HttpRequestMessage request;

//        public FunctionalTestTemplate(WebApplicationFactory<Startup> factory) :
//            base(factory.CreateClient())
//        {
//            request = new HttpRequestMessage
//            {
//                Method = HttpMethod.Get,
//                Version = new Version("1.0"),
//                RequestUri = GetUri(ApiRouteEnum.Territory)
//            };
//        }

//        public void Dispose()
//        {
//            request = null;
//            Token = null;
//        }

//        [Fact]
//        public void Example()
//        {
//            Assert.NotNull(request);
//        }
//    }
//}
