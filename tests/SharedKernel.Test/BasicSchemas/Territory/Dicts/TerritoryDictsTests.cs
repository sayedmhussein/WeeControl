using System;
using WeeControl.SharedKernel.BasicSchemas.Territory.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Territory.Interfaces;
using Xunit;

namespace WeeControl.SharedKernel.Test.Schemas.Territory.Dicts
{
    public class TerritoryDictsTests
    {
        [Fact]
        public void Country_EnsureThatEveryEnumIsLocatedInJsonFile()
        {
            ITerritoryDicts values = new TerritoryDicts();

            foreach (var uri in values.Country)
            {
                Assert.True(uri.Value != null, string.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }
        }
    }
}
