using WeeControl.SharedKernel.EntityGroups.Employee.Attributes;
using WeeControl.SharedKernel.EntityGroups.Employee.Enums;
using WeeControl.SharedKernel.EntityGroups.Employee.Interfaces;
using WeeControl.SharedKernel.Test.TestHelpers;
using Xunit;

namespace WeeControl.SharedKernel.Test.EntityGroups.Employee.Attributes
{
    public class EmployeeAttributeTests
    {
        private readonly IEmployeeAttribute attribute;
        
        public EmployeeAttributeTests()
        {
            attribute = new EmployeeAttribute();
        }

        [Fact]
        public void GetPersonalTitleTests()
        {
            AttributeTester.MyDelegate<PersonalTitleEnum, string> del = attribute.GetPersonalTitle;
            new AttributeTester().Test(del);
        }

        [Fact]
        public void GetPersonalGenderTests()
        {
            AttributeTester.MyDelegate<PersonalGenderEnum, string> del = attribute.GetPersonalGender;
            new AttributeTester().Test(del);
        }

        [Fact]
        public void GetPersonalIdentityTests()
        {
            AttributeTester.MyDelegate<IdentityTypeEnum, string> del = attribute.GetPersonalIdentity;
            new AttributeTester().Test(del);
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
