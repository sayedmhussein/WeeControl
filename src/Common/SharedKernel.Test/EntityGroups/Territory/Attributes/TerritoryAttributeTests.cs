using WeeControl.Common.SharedKernel.EntityGroups.Territory.Attributes;
using WeeControl.Common.SharedKernel.EntityGroups.Territory.Enums;
using WeeControl.Common.SharedKernel.EntityGroups.Territory.Interfaces;
using WeeControl.Common.SharedKernel.Test.TestHelpers;
using Xunit;

namespace WeeControl.Common.SharedKernel.Test.EntityGroups.Territory.Attributes
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
