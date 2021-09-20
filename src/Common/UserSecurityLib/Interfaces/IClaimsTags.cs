using WeeControl.Common.UserSecurityLib.Enums;

namespace WeeControl.Common.UserSecurityLib.Interfaces
{
    public interface IClaimsTags
    {
        public string GetClaimType(ClaimTypeEnum claimType);
        public string GetClaimTag(ClaimTagEnum claimTag);
    }
}