using System;
using System.Linq;
using WeeControl.SharedKernel.EntityGroups.Employee.Attributes;
using WeeControl.SharedKernel.EntityGroups.Employee.Enums;
using WeeControl.SharedKernel.EntityGroups.Employee.Interfaces;
using Xunit;

namespace WeeControl.SharedKernel.Test.Aggregates.Employee
{
    public class EmployeeListsTests
    {
        private readonly IEmployeeAttribute attribute;

        public EmployeeListsTests()
        {
            attribute = new EmployeeAttribute();
        }

        [Fact]
        public void GetPersonalTitles_AllEnumMustExist()
        {
            foreach (var e in Enum.GetValues(typeof(PersonalTitleEnum)).Cast<PersonalTitleEnum>())
            {
                var item = attribute.GetPersonalTitle(e);

                Assert.False(string.IsNullOrEmpty(item), string.Format("\"{0}\" Enum don't have value in JSON file.", item));
            }
        }
    }
}
