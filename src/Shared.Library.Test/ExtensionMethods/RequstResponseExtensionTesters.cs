using System;
using MySystem.Shared.Library.Dto;
using MySystem.Shared.Library.Dto.EntityV1;
using MySystem.Shared.Library.ExtensionMethod;
using Newtonsoft.Json;
using Xunit;

namespace MySystem.Shared.Library.Test.ExtensionMethods
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
