using Newtonsoft.Json;
using WeeControl.SharedKernel.BasicSchemas.Common.DtosV1;
using WeeControl.SharedKernel.Extensions;
using Xunit;

namespace WeeControl.SharedKernel.Test.Extensions
{
    public class DtoSerializationExtensionTesters
    {
        [Fact]
        public void WhenSerializingARequest_JsonStringShouldBeProduced()
        {
            var str = new RequestMetadataV1().SerializeToJson();
            Assert.NotEmpty(str);
        }

        [Fact]
        public void WhenSerializingARequestThenDeserialzed_ObjectShouldBeSame()
        {
            var token = "SomeToken";
            var str = new RequestMetadataV1() { Device = token }.SerializeToJson();

            var obj = JsonConvert.DeserializeObject<RequestMetadataV1>(str);

            Assert.Equal(token, obj.Device);
        }
    }
}
