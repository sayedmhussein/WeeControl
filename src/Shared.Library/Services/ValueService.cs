using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MySystem.SharedKernel.Configuration;
using MySystem.SharedKernel.Enumerators;
using MySystem.SharedKernel.Interfaces;

namespace MySystem.SharedKernel.Services
{
    public class ValueService : IValuesService
    {
        public ImmutableDictionary<ApiRouteEnum, string> ApiRoute { get; private set; }

        public ImmutableDictionary<ClaimTypeEnum, string> ClaimType { get; private set; }

        public ImmutableDictionary<ClaimTagEnum, string> ClaimTag { get; private set; }

        public ImmutableDictionary<PersonalTitleEnum, string> PersonTitle { get; private set; }

        public ImmutableDictionary<PersonalGenderEnum, string> PersonGender { get; private set; }

        public ImmutableDictionary<CountryEnum, string> Country { get; private set; }

        public ValueService()
        {
            dynamic obj = Config.AppSettingObject;

            var apiRoute = new Dictionary<ApiRouteEnum, string>();
            foreach (var e in Enum.GetValues(typeof(ApiRouteEnum)).Cast<ApiRouteEnum>())
            {
                string value = obj.ApiRoute[e.ToString()];
                apiRoute.Add(e, value);
            }
            ApiRoute = apiRoute.ToImmutableDictionary();

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

            var country = new Dictionary<CountryEnum, string>();
            foreach (var e in Enum.GetValues(typeof(CountryEnum)).Cast<CountryEnum>())
            {
                string value = obj.Country[e.ToString()];
                country.Add(e, value);
            }
            Country = country.ToImmutableDictionary();
        }
    }
}
