using MySystem.SharedKernel.EntityV1Dtos.Territory;
using MySystem.SharedKernel.ExtensionMethods;
using Newtonsoft.Json;
using Xunit;

namespace MySystem.SharedKernel.Test.ExtensionMethods
{
    public class DtoSerializationExtensionTesters
    {
        [Fact]
        public void WhenSerializingARequest_JsonStringShouldBeProduced()
        {
            var str = new TerritoryDto().SerializeToJson();
            Assert.NotEmpty(str);
        }

        [Fact]
        public void WhenSerializingARequestThenDeserialzed_ObjectShouldBeSame()
        {
            var token = "SomeToken";
            var str = new TerritoryDto() { Name= token }.SerializeToJson();

            var obj = JsonConvert.DeserializeObject<TerritoryDto>(str);

            Assert.Equal(token, obj.Name);
        }
    }
}
