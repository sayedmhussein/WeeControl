using System;
using System.Collections.Generic;
using System.Linq;
using WeeControl.SharedKernel.Aggregates.Employee.Enums;
using WeeControl.SharedKernel.Configurations;

namespace WeeControl.SharedKernel.Aggregates.Employee
{
    public class EmployeeLists : AppSettings, IEmployeeLists
    {
        private Dictionary<PersonalTitleEnum, string> personTitle;
        private Dictionary<PersonalGenderEnum, string> personGender;
        private Dictionary<IdentityTypeEnum, string> identityType;

        private Dictionary<ClaimTypeEnum, string> claimType;
        private Dictionary<ClaimTagEnum, string> claimTag;

        public string GetClaimTag(ClaimTagEnum tag)
        {
            if (claimTag == null)
            {
                claimTag = new Dictionary<ClaimTagEnum, string>();
                foreach (var e in Enum.GetValues(typeof(ClaimTagEnum)).Cast<ClaimTagEnum>())
                {
                    string value = json.ClaimTag[e.ToString()];
                    claimTag.Add(e, value);
                }
            }

            return claimTag[tag];
        }

        public string GetClaimType(ClaimTypeEnum type)
        {
            if (claimType == null)
            {
                claimType = new Dictionary<ClaimTypeEnum, string>();
                foreach (var e in Enum.GetValues(typeof(ClaimTypeEnum)).Cast<ClaimTypeEnum>())
                {
                    string value = json.ClaimType[e.ToString()];
                    claimType.Add(e, value);
                }
            }

            return claimType[type];
        }

        public string GetPersonalGender(PersonalGenderEnum gender)
        {
            if (personGender == null)
            {
                personGender = new Dictionary<PersonalGenderEnum, string>();
                foreach (var e in Enum.GetValues(typeof(PersonalGenderEnum)).Cast<PersonalGenderEnum>())
                {
                    string value = json.PersonalGender[e.ToString()];
                    personGender.Add(e, value);
                }
            }

            return personGender[gender];
        }

        public string GetPersonalIdentity(IdentityTypeEnum identity)
        {
            if (identityType == null)
            {
                identityType = new Dictionary<IdentityTypeEnum, string>();
                foreach (var e in Enum.GetValues(typeof(IdentityTypeEnum)).Cast<IdentityTypeEnum>())
                {
                    string value = json.IdentityType[e.ToString()];
                    identityType.Add(e, value);
                }
            }

            return identityType[identity];
        }

        public string GetPersonalTitle(PersonalTitleEnum title)
        {
            if (personTitle == null)
            {
                personTitle = new Dictionary<PersonalTitleEnum, string>();
                foreach (var e in Enum.GetValues(typeof(PersonalTitleEnum)).Cast<PersonalTitleEnum>())
                {
                    string value = json.PersonalTitle[e.ToString()];
                    personTitle.Add(e, value);
                }
            }

            return personTitle[title];
        }
    }
}
