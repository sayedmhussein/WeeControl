using System;
using System.Reflection;
using MySystem.SharedKernel.Services;
using Xunit;
namespace MySystem.SharedKernel.Test.Services
{
    public class EmbeddedResourcesManagerTests
    {
        [Fact]
        public void WhenGettingAppSettingObject_MustNotBeNull()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(EmbeddedResourcesManager));
            string resource = "MySystem.SharedKernel.Configuration.appsettings.json";

            dynamic obj = new EmbeddedResourcesManager(assembly).GetSerializedAsJson(resource);

            Assert.NotNull(obj);
        }

        [Fact]
        public void GetCurrentNamespace()
        {
            var bla = MethodBase.GetCurrentMethod().DeclaringType.Namespace;

            Assert.NotEmpty(bla);
        }
    }
}
