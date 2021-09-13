using WeeControl.SharedKernel.EntityGroups.Employee.Enums;

namespace WeeControl.SharedKernel.EntityGroups.Employee.Interfaces
{
    public interface IEmployeeAttribute
    {
        public string GetPersonalTitle(PersonalTitleEnum title);
        public string GetPersonalGender(PersonalGenderEnum gender);
        public string GetPersonalIdentity(IdentityTypeEnum identity);

        public string GetClaimType(ClaimTypeEnum claimType);
        public string GetClaimTag(ClaimTagEnum claimTag);
    }
}
