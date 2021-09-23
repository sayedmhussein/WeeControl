using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Obsolute.EntityGroups.Employee.Enums;

namespace WeeControl.Common.SharedKernel.Obsolute.EntityGroups.Employee.Interfaces
{
    public interface IEmployeeAttribute : IEntityAttribute
    {
        public string GetPersonalTitle(PersonalTitleEnum title);
        public string GetPersonalGender(PersonalGenderEnum gender);
        public string GetPersonalIdentity(IdentityTypeEnum identity);

        
    }
}
