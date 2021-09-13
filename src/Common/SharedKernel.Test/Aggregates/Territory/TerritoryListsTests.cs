using System;
using System.Linq;
using WeeControl.SharedKernel.EntityGroups.Territory.Attributes;
using WeeControl.SharedKernel.EntityGroups.Territory.Enums;
using WeeControl.SharedKernel.EntityGroups.Territory.Interfaces;
using Xunit;

namespace WeeControl.SharedKernel.Test.Aggregates.Territory
{
    public class TerritoryListsTests
    {
        private readonly ITerritoryAttribute attribute;

        public TerritoryListsTests()
        {
            attribute = new TerritoryAttribute();
        }

        [Fact]
        public void GetCountryNames_AllEnumMustExist()
        {
            foreach (var e in Enum.GetValues(typeof(CountryEnum)).Cast<CountryEnum>())
            {
                var country = attribute.GetCountryName(e);

                Assert.False(string.IsNullOrEmpty(country), string.Format("\"{0}\" Enum don't have value in JSON file.", country));
            }
        }
    }
}
