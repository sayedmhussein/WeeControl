using WeeControl.Common.UserSecurityLib.Enums;
using WeeControl.Common.UserSecurityLib.Interfaces;
using WeeControl.Common.UserSecurityLib.Services;
using WeeControl.Common.UserSecurityLib.Test.TestHelpers;
using Xunit;

namespace WeeControl.Common.UserSecurityLib.Test.Services
{
    public class ClaimTagListTests
    {
        private readonly IUserClaimService attribute;

        public ClaimTagListTests()
        {
            attribute = new UserClaimService();
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