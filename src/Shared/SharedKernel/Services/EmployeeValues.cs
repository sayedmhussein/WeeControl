using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using MySystem.SharedKernel.Enumerators.Employee;
using MySystem.SharedKernel.Interfaces.Values;

namespace MySystem.SharedKernel.Services
{
    public class EmployeeValues : IEmployeeValues
    {
        public ImmutableDictionary<ClaimTypeEnum, string> ClaimType { get; private set; }

        public ImmutableDictionary<ClaimTagEnum, string> ClaimTag { get; private set; }

        public ImmutableDictionary<PersonalTitleEnum, string> PersonTitle { get; private set; }

        public ImmutableDictionary<PersonalGenderEnum, string> PersonGender { get; private set; }

        public ImmutableDictionary<IdentityTypeEnum, string> IdentityType { get; private set; }

        public EmployeeValues()
        {
            dynamic obj = new EmbeddedResourcesManager(
                Assembly.GetExecutingAssembly())
                .GetSerializedAsJson("MySystem.SharedKernel.Configuration.appsettings.json");

            var claimType = new Dictionary<ClaimTypeEnum, string>();
            foreach (var e in Enum.GetValues(typeof(ClaimTypeEnum)).Cast<ClaimTypeEnum>())
            {
                string value = obj.ClaimType[e.ToString()];
                claimType.Add(e, value);
            }
            ClaimType = claimType.ToImmutableDictionary();

            var claimTag = new Dictionary<ClaimTagEnum, string>();
            foreach (var e in Enum.GetValues(typeof(ClaimTagEnum)).Cast<ClaimTagEnum>())
            {
                string value = obj.ClaimTag[e.ToString()];
                claimTag.Add(e, value);
            }
            ClaimTag = claimTag.ToImmutableDictionary();

            var personTitle = new Dictionary<PersonalTitleEnum, string>();
            foreach (var e in Enum.GetValues(typeof(PersonalTitleEnum)).Cast<PersonalTitleEnum>())
            {
                string value = obj.PersonalTitle[e.ToString()];
                personTitle.Add(e, value);
            }
            PersonTitle = personTitle.ToImmutableDictionary();

            var personGender = new Dictionary<PersonalGenderEnum, string>();
            foreach (var e in Enum.GetValues(typeof(PersonalGenderEnum)).Cast<PersonalGenderEnum>())
            {
                string value = obj.PersonalGender[e.ToString()];
                personGender.Add(e, value);
            }
            PersonGender = personGender.ToImmutableDictionary();

            var identityType = new Dictionary<IdentityTypeEnum, string>();
            foreach (var e in Enum.GetValues(typeof(IdentityTypeEnum)).Cast<IdentityTypeEnum>())
            {
                string value = obj.IdentityType[e.ToString()];
                identityType.Add(e, value);
            }
            IdentityType = identityType.ToImmutableDictionary();
        }
    }
}
