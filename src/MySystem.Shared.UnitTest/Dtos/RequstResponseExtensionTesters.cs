﻿using System;
using Newtonsoft.Json;
using Sayed.MySystem.Shared.Dtos;
using Sayed.MySystem.Shared.Dtos.V1;
using Xunit;

namespace Sayed.MySystem.Shared.UnitTest.Dtos
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
