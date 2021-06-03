using System;
using System.Collections.Generic;
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
        public void ForEachClaimTypeInList_EachTypeValueMustBeUnique()
        {
            var dict = new Dictionary<string, string>();

            foreach (var type in Claims.Types)
            {
                dict.Add(type.Value, "");
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

        [Fact]
        public void ForEachClaimTagInList_EachTagValueMustBeUnique()
        {
            var dict = new Dictionary<string, string>();

            foreach (var type in Claims.Tags)
            {
                dict.Add(type.Value, "");
            }
        }
    }
}
