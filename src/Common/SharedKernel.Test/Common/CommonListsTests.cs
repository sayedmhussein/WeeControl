using System;
using System.Linq;
using WeeControl.SharedKernel.Helpers;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.Obsolutes;
using Xunit;

namespace WeeControl.SharedKernel.Test.Common
{
    public class CommonListsTests
    {
        private readonly ICommonLists lists;

        public CommonListsTests()
        {
            lists = new CommonLists();
        }

        [Fact]
        public void GetCountryNames_AllEnumMustExist()
        {
            foreach (var e in Enum.GetValues(typeof(ApiRouteEnum)).Cast<ApiRouteEnum>())
            {
                var route = lists.GetRoute(e);

                Assert.False(string.IsNullOrEmpty(route), string.Format("\"{0}\" Enum don't have value in JSON file.", route));
            }
        }
    }
}
