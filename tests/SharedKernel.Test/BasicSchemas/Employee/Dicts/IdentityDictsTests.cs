using System;
using WeeControl.SharedKernel.BasicSchemas.Employee.Dicts;
using Xunit;

namespace WeeControl.SharedKernel.Test.Schemas.Employee.Dicts
{
    public class IdentityDictsTests
    {
        [Fact]
        public void ClaimType_EnsureThatEveryEnumIsLocatedInJsonFile()
        {
            IIdentityDicts values = new IdentityDicts();

            foreach (var uri in values.IdentityType)
            {
                Assert.True(uri.Value != null, string.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }
        }
    }
}
