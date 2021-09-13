using System.Collections.Generic;
using WeeControl.SharedKernel.EntityGroups.Employee.Enums;
using WeeControl.SharedKernel.EntityGroups.Employee.Interfaces;
using WeeControl.SharedKernel.Helpers;

namespace WeeControl.SharedKernel.EntityGroups.Employee.Attributes
{
    public class EmployeeAttribute : AttributesReader, IEmployeeAttribute
    {
        private Dictionary<PersonalTitleEnum, string> personTitle;
        private Dictionary<PersonalGenderEnum, string> personGender;
        private Dictionary<IdentityTypeEnum, string> identityType;

        private Dictionary<ClaimTypeEnum, string> claimType;
        private Dictionary<ClaimTagEnum, string> claimTag;

        public EmployeeAttribute() : base(typeof(EmployeeAttribute).Namespace)
        {
        }
        public string GetClaimTag(ClaimTagEnum tag)
        {
            PopulateDictionary(ref claimTag, "ClaimTags");
            return claimTag[tag];
        }

        public string GetClaimType(ClaimTypeEnum type)
        {
            PopulateDictionary(ref claimType, "ClaimTypes");
            return claimType[type];
        }

        public string GetPersonalGender(PersonalGenderEnum gender)
        {
            PopulateDictionary(ref personGender, "Genders");
            return personGender[gender];
        }

        public string GetPersonalIdentity(IdentityTypeEnum identity)
        {
            PopulateDictionary(ref identityType, "IdentityTypes");
            return identityType[identity];
        }

        public string GetPersonalTitle(PersonalTitleEnum title)
        {
            PopulateDictionary(ref personTitle, "Titles");
            return personTitle[title];
        }
    }
}
