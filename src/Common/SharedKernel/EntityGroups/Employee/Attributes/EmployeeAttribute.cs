using System.Collections.Generic;
using WeeControl.Common.SharedKernel.EntityGroups.Employee.Enums;
using WeeControl.Common.SharedKernel.EntityGroups.Employee.Interfaces;
using WeeControl.Common.SharedKernel.Services;

namespace WeeControl.Common.SharedKernel.EntityGroups.Employee.Attributes
{
    public class EmployeeAttribute : IEmployeeAttribute
    {
        private readonly AppSettingReader appSettingReader;
        
        private Dictionary<PersonalTitleEnum, string> personTitle;
        private Dictionary<PersonalGenderEnum, string> personGender;
        private Dictionary<IdentityTypeEnum, string> identityType;

        
        public EmployeeAttribute()
        {
            appSettingReader = new AppSettingReader(typeof(EmployeeAttribute).Namespace, "attributes.json");
        }
        
        public string GetPersonalGender(PersonalGenderEnum gender)
        {
            appSettingReader.PopulateAttribute(ref personGender, "Genders");
            return personGender[gender];
        }

        public string GetPersonalIdentity(IdentityTypeEnum identity)
        {
            appSettingReader.PopulateAttribute(ref identityType, "IdentityTypes");
            return identityType[identity];
        }

        public string GetPersonalTitle(PersonalTitleEnum title)
        {
            appSettingReader.PopulateAttribute(ref personTitle, "Titles");
            return personTitle[title];
        }
    }
}
