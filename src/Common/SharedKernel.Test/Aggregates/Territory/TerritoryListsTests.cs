using System;
using System.Linq;
using WeeControl.SharedKernel.EntityGroup.Territory;
using WeeControl.SharedKernel.EntityGroup.Territory.Enums;
using Xunit;

namespace WeeControl.SharedKernel.Test.Aggregates.Territory
{
    public class TerritoryListsTests
    {
        private readonly ITerritoryLists lists;

        public TerritoryListsTests()
        {
            lists = new TerritoryLists();
        }

        [Fact]
        public void GetCountryNames_AllEnumMustExist()
        {
            foreach (var e in Enum.GetValues(typeof(CountryEnum)).Cast<CountryEnum>())
            {
                var country = lists.GetCountryName(e);

                Assert.False(string.IsNullOrEmpty(country), string.Format("\"{0}\" Enum don't have value in JSON file.", country));
            }
        }
    }
}
