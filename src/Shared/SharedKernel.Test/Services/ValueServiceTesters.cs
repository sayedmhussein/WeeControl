using System;
using MySystem.SharedKernel.Interfaces.Values;
using MySystem.SharedKernel.Services;
using Xunit;

namespace MySystem.SharedKernel.Test.Services
{
    public class ValueServiceTesters
    {
        [Fact]
        public void ApiRoute_EnsureThatEveryEnumIsLocatedInJsonFile()
        {
            ICommonValues values = new CommonValues();

            foreach (var uri in values.ApiRoute)
            {
                Assert.True(uri.Value!=null, String.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }
        }

        [Fact]
        public void ClaimType_EnsureThatEveryEnumIsLocatedInJsonFile()
        {
            IEmployeeValues values = new EmployeeValues();

            foreach (var uri in values.ClaimType)
            {
                Assert.True(uri.Value != null, String.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }

            foreach (var uri in values.ClaimTag)
            {
                Assert.True(uri.Value != null, String.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }

            foreach (var uri in values.PersonTitle)
            {
                Assert.True(uri.Value != null, String.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }

            foreach (var uri in values.PersonGender)
            {
                Assert.True(uri.Value != null, string.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }

            foreach (var uri in values.IdentityType)
            {
                Assert.True(uri.Value != null, string.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }
        }

        [Fact]
        public void Country_EnsureThatEveryEnumIsLocatedInJsonFile()
        {
            ITerritoryValues values = new TerritoryValues();

            foreach (var uri in values.Country)
            {
                Assert.True(uri.Value != null, string.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }
        }
    }
}
