//using System;
//using System.Collections.Generic;
//using System.Linq;
//using MySystem.SharedKernel.Enumerators;
//using Xunit;

//namespace MySystem.SharedKernel.Test.Entites.Public.Constants
//{
//    public class ClaimsTests
//    {
//        [Fact]
//        public void ForEachClaimTypeInEnum_TypesShouldBeInDictionary()
//        {
//            foreach (var claimType in Enum.GetValues(typeof(ClaimTypeEnum)).Cast<ClaimTypeEnum>())
//            {
//                Assert.NotEmpty(ClaimService.Types[claimType]);
//            }
//        }

//        [Fact]
//        public void ForEachClaimTypeInList_EachTypeValueMustBeUnique()
//        {
//            var dict = new Dictionary<string, string>();

//            foreach (var type in ClaimService.Types)
//            {
//                dict.Add(type.Value, "");
//            }
//        }

//        [Fact]
//        public void ForEachClaimTagInEnum_TagsShouldBeInDictionary()
//        {
//            foreach (var claimTag in Enum.GetValues(typeof(ClaimTagEnum)).Cast<ClaimTagEnum>())
//            {
//                Assert.NotEmpty(ClaimService.Tags[claimTag]);
//            }
//        }

//        [Fact]
//        public void ForEachClaimTagInList_EachTagValueMustBeUnique()
//        {
//            var dict = new Dictionary<string, string>();

//            foreach (var type in ClaimService.Tags)
//            {
//                dict.Add(type.Value, "");
//            }
//        }
//    }
//}
