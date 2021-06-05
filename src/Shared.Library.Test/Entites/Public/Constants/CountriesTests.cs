using System;
using System.Linq;
using Xunit;
using static MySystem.SharedKernel.Entities.Public.Constants.Counties;

namespace MySystem.SharedKernel.Test.Entites.Public.Constants
{
    public class CountriesTests
    {
        [Fact]
        public void ForEachClaimTypeInEnum_TypesShouldBeInDictionary()
        {
            foreach (var country in Enum.GetValues(typeof(Name)).Cast<Name>())
            {
                Assert.NotEmpty(List[country]);
            }
        }
    }
}
