using System;
using MySystem.SharedKernel.Dto.V1;
using MySystem.SharedKernel.ExtensionMethod;
using Newtonsoft.Json;
using Xunit;

namespace MySystem.SharedKernel.Test.ExtensionMethods
{
    public class RequstResponseExtensionTesters
    {
        [Fact]
        public void WhenSerializingARequest_JsonStringShouldBeProduced()
        {
            var str = new RequestDto<string>("DeviceId", "SomeData").SerializeToJson();
            Assert.NotEmpty(str);
        }

        [Fact]
        public void WhenSerializingARequestThenDeserialzed_ObjectShouldBeSame()
        {
            var token = "SomeToken";
            var str = new RequestDto<string>("DeviceId", token).SerializeToJson();

            var obj = JsonConvert.DeserializeObject<RequestDto<string>>(str);

            Assert.Equal(token, obj.Payload);
        }
    }
}
