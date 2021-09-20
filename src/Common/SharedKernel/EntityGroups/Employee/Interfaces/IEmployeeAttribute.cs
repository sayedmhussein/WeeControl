using WeeControl.Common.SharedKernel.EntityGroups.Employee.Enums;
using WeeControl.Common.SharedKernel.Interfaces;

namespace WeeControl.Common.SharedKernel.EntityGroups.Employee.Interfaces
{
    public interface IEmployeeAttribute : IEntityAttribute
    {
        public string GetPersonalTitle(PersonalTitleEnum title);
        public string GetPersonalGender(PersonalGenderEnum gender);
        public string GetPersonalIdentity(IdentityTypeEnum identity);

        public string GetClaimType(ClaimTypeEnum claimType);
        public string GetClaimTag(ClaimTagEnum claimTag);
    }
}
