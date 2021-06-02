using System;
using System.Linq;
using MySystem.SharedKernel.Entities.Public.Constants;
using Xunit;
using static MySystem.SharedKernel.Entities.Public.Constants.Claims;

namespace MySystem.SharedKernel.Test.Entites.Public.Constants
{
    public class ClaimsTests
    {
        [Fact]
        public void ForEachClaimTypeInEnum_TypesShouldBeInDictionary()
        {
            foreach (var claimType in Enum.GetValues(typeof(ClaimType)).Cast<ClaimType>())
            {
                Assert.NotEmpty(Claims.Types[claimType]);
            }
        }

        [Fact]
        public void ForEachClaimTagInEnum_TagsShouldBeInDictionary()
        {
            foreach (var claimTag in Enum.GetValues(typeof(ClaimTag)).Cast<ClaimTag>())
            {
                Assert.NotEmpty(Claims.Tags[claimTag]);
            }
        }
    }
}
