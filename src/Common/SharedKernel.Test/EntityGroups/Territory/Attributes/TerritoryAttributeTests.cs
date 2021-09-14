using WeeControl.SharedKernel.EntityGroups.Territory.Attributes;
using WeeControl.SharedKernel.EntityGroups.Territory.Enums;
using WeeControl.SharedKernel.EntityGroups.Territory.Interfaces;
using WeeControl.SharedKernel.Test.TestHelpers;
using Xunit;

namespace WeeControl.SharedKernel.Test.EntityGroups.Territory.Attributes
{
    public class TerritoryAttributeTests
    {
        private readonly ITerritoryAttribute attribute;

        public TerritoryAttributeTests()
        {
            attribute = new TerritoryAppSetting();
        }

        [Fact]
        public void GetCountryName_s_AllEnumMustExist()
        {
            AttributeTester.MyDelegate<CountryEnum, string>  myDelegate = attribute.GetCountryName;
            new AttributeTester().Test(myDelegate);
        }
    }
}
