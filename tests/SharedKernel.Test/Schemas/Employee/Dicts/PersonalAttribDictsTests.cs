using System;
using WeeControl.SharedKernel.CommonSchemas.Employee.Dicts;
using Xunit;

namespace WeeControl.SharedKernel.Test.Schemas.Employee.Dicts
{
    public class PersonalAttribDictsTests
    {
        [Fact]
        public void ClaimType_EnsureThatEveryEnumIsLocatedInJsonFile()
        {
            IPersonalAttribDicts values = new PersonalAttribDicts();

            foreach (var uri in values.PersonTitle)
            {
                Assert.True(uri.Value != null, String.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }

            foreach (var uri in values.PersonGender)
            {
                Assert.True(uri.Value != null, string.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }
        }
    }
}
