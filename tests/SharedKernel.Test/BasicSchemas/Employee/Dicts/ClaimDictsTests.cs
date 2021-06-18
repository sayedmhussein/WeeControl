using System;
using WeeControl.SharedKernel.BasicSchemas.Employee.Dicts;
using Xunit;

namespace WeeControl.SharedKernel.Test.Schemas.Employee.Dicts
{
    public class ClaimDictsTests
    {
        [Fact]
        public void ClaimType_EnsureThatEveryEnumIsLocatedInJsonFile()
        {
            IClaimDicts values = new ClaimDicts();

            foreach (var uri in values.ClaimType)
            {
                Assert.True(uri.Value != null, String.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }

            foreach (var uri in values.ClaimTag)
            {
                Assert.True(uri.Value != null, String.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }
        }
    }
}
