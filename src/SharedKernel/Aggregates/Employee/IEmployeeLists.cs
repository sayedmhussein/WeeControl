using WeeControl.SharedKernel.Aggregates.Employee.Enums;

namespace WeeControl.SharedKernel.Aggregates.Employee
{
    public interface IEmployeeLists
    {
        public string GetPersonalTitle(PersonalTitleEnum title);
        public string GetPersonalGender(PersonalGenderEnum gender);
        public string GetPersonalIdentity(IdentityTypeEnum identity);

        public string GetClaimType(ClaimTypeEnum claimType);
        public string GetClaimTag(ClaimTagEnum claimTag);
    }
}
