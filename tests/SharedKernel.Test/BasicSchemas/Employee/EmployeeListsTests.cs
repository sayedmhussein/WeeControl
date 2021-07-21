﻿using System;
using System.Linq;
using WeeControl.SharedKernel.BasicSchemas.Employee;
using WeeControl.SharedKernel.BasicSchemas.Employee.Enums;
using Xunit;

namespace WeeControl.SharedKernel.Test.BasicSchemas.Employee
{
    public class EmployeeListsTests
    {
        private readonly IEmployeeLists lists;

        public EmployeeListsTests()
        {
            lists = new EmployeeLists();
        }

        [Fact]
        public void GetPersonalTitles_AllEnumMustExist()
        {
            foreach (var e in Enum.GetValues(typeof(PersonalTitleEnum)).Cast<PersonalTitleEnum>())
            {
                var item = lists.GetPersonalTitle(e);

                Assert.False(string.IsNullOrEmpty(item), string.Format("\"{0}\" Enum don't have value in JSON file.", item));
            }
        }
    }
}