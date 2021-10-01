using Newtonsoft.Json;
using Xunit;

namespace WeeControl.SharedKernel.Test.Extensions
{
    public class DtoSerializationExtensionTesters
    {
        // [Fact]
        // public void WhenSerializingARequest_JsonStringShouldBeProduced()
        // {
        //     var str = new Dto() { CountryCode = "EGP", Name = "EGP" }.SerializeToJson();
        //     Assert.NotEmpty(str);
        // }
        //
        // [Fact]
        // public void WhenSerializingARequestThenDeserialzed_ObjectShouldBeSame()
        // {
        //     var name = "EGP";
        //     var str = new Dto() { CountryCode = "EGP", Name = "EGP" }.SerializeToJson();
        //
        //     var obj = JsonConvert.DeserializeObject<Dto>(str);
        //
        //     Assert.Equal(name, obj.CountryCode);
        // }
    }
}
