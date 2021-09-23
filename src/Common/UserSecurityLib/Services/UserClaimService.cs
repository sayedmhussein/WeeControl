using System.Collections.Generic;
using WeeControl.Common.UserSecurityLib.Enums;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Common.UserSecurityLib.Services
{
    public class UserClaimService : IUserClaimService
    {
        private readonly ClaimsTagsReader appSettingReader;
        
        private Dictionary<ClaimTypeEnum, string> claimType;
        private Dictionary<ClaimTagEnum, string> claimTag;

        public UserClaimService()
        {
            appSettingReader = new ClaimsTagsReader(typeof(DependencyInjection).Namespace, "appsettings.json");
        }
        
        public string GetClaimTag(ClaimTagEnum tag)
        {
            appSettingReader.PopulateAttribute(ref claimTag, "ClaimTags");
            return claimTag[tag];
        }

        public string GetClaimType(ClaimTypeEnum type)
        {
            appSettingReader.PopulateAttribute(ref claimType, "ClaimTypes");
            return claimType[type];
        }
    }
}