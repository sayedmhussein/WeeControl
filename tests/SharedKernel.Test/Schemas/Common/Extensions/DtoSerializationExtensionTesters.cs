using Newtonsoft.Json;
using WeeControl.SharedKernel.CommonSchemas.Common.DtosV1;
using WeeControl.SharedKernel.CommonSchemas.Common.Extensions;
using Xunit;

namespace WeeControl.SharedKernel.Test.Schemas.Common.Extensions
{
    public class DtoSerializationExtensionTesters
    {
        [Fact]
        public void WhenSerializingARequest_JsonStringShouldBeProduced()
        {
            var str = new RequestMetadata().SerializeToJson();
            Assert.NotEmpty(str);
        }

        [Fact]
        public void WhenSerializingARequestThenDeserialzed_ObjectShouldBeSame()
        {
            var token = "SomeToken";
            var str = new RequestMetadata() { Device = token }.SerializeToJson();

            var obj = JsonConvert.DeserializeObject<RequestMetadata>(str);

            Assert.Equal(token, obj.Device);
        }
    }
}
