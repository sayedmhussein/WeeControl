using System;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.Services;
using Xunit;

namespace MySystem.SharedKernel.Test.Services
{
    public class ValueServiceTesters
    {
        private readonly ISharedValues uriService;

        public ValueServiceTesters()
        {
            uriService = new SharedValues();
        }

        [Fact]
        public void ApiRoute_EnsureThatEveryEnumIsLocatedInJsonFile()
        {
            foreach (var uri in uriService.ApiRoute)
            {
                Assert.True(uri.Value!=null, String.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }
        }

        [Fact]
        public void ClaimType_EnsureThatEveryEnumIsLocatedInJsonFile()
        {
            foreach (var uri in uriService.ClaimType)
            {
                Assert.True(uri.Value != null, String.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }
        }

        [Fact]
        public void ClaimTag_EnsureThatEveryEnumIsLocatedInJsonFile()
        {
            foreach (var uri in uriService.ClaimTag)
            {
                Assert.True(uri.Value != null, String.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }
        }

        [Fact]
        public void PersonalTitle_EnsureThatEveryEnumIsLocatedInJsonFile()
        {
            foreach (var uri in uriService.PersonTitle)
            {
                Assert.True(uri.Value != null, String.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }
        }

        [Fact]
        public void PersonalGender_EnsureThatEveryEnumIsLocatedInJsonFile()
        {
            foreach (var uri in uriService.PersonGender)
            {
                Assert.True(uri.Value != null, String.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }
        }

        [Fact]
        public void Country_EnsureThatEveryEnumIsLocatedInJsonFile()
        {
            foreach (var uri in uriService.Country)
            {
                Assert.True(uri.Value != null, String.Format("\"{0}\" Enum don't have value in JSON file.", uri.Key));
            }
        }
    }
}
