using Newtonsoft.Json;
using WeeControl.Common.SharedKernel.Extensions;
using WeeControl.Common.SharedKernel.Obsolute.Territory;
using Xunit;

namespace WeeControl.Common.SharedKernel.Test.Extensions
{
    public class DtoSerializationExtensionTesters
    {
        [Fact]
        public void WhenSerializingARequest_JsonStringShouldBeProduced()
        {
            var str = new TerritoryDto() { CountryId = "EGP", Name = "EGP" }.SerializeToJson();
            Assert.NotEmpty(str);
        }

        [Fact]
        public void WhenSerializingARequestThenDeserialzed_ObjectShouldBeSame()
        {
            var name = "EGP";
            var str = new TerritoryDto() { CountryId = "EGP", Name = "EGP" }.SerializeToJson();

            var obj = JsonConvert.DeserializeObject<TerritoryDto>(str);

            Assert.Equal(name, obj.CountryId);
        }
    }
}
