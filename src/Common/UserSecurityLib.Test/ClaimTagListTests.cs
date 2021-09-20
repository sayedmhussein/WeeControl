using WeeControl.Common.UserSecurityLib.Enums;
using WeeControl.Common.UserSecurityLib.Interfaces;
using WeeControl.Common.UserSecurityLib.Test.TestHelpers;
using Xunit;

namespace WeeControl.Common.UserSecurityLib.Test
{
    public class ClaimTagListTests
    {
        private readonly IClaimsTags attribute;

        public ClaimTagListTests()
        {
            attribute = new ClaimsTagsList();
        }
        
        [Fact]
        public void GetClaimTypeTests()
        {
            AttributeTester.MyDelegate<ClaimTypeEnum, string> del = attribute.GetClaimType;
            new AttributeTester().Test(del);
        }

        [Fact]
        public void GetClaimTagTests()
        {
            AttributeTester.MyDelegate<ClaimTagEnum, string> del = attribute.GetClaimTag;
            new AttributeTester().Test(del);
        }
    }
}