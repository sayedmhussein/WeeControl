using System;
using WeeControl.SharedKernel.BasicSchemas.Common.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Common.Interfaces;
using Xunit;

namespace WeeControl.SharedKernel.Test.Schemas.Common.Dicts
{
    public class ApiDictsTests
    {
        [Fact]
        public void ApiRoute_EnsureThatEveryEnumIsLocatedInJsonFile()
        {
            IApiDicts values = new ApiDicts();

            foreach (var uri in values.ApiRoute)
            {
                Assert.True(uri.Value != null, String.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }
        }
    }
}
