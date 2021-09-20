using System.Collections.Generic;
using WeeControl.Common.UserSecurityLib.Enums;
using WeeControl.Common.UserSecurityLib.Interfaces;
using WeeControl.Common.UserSecurityLib.Services;

namespace WeeControl.Common.UserSecurityLib
{
    public class ClaimsTagsList : IClaimsTags
    {
        private readonly ClaimsTagsReader appSettingReader;
        
        private Dictionary<ClaimTypeEnum, string> claimType;
        private Dictionary<ClaimTagEnum, string> claimTag;

        public ClaimsTagsList()
        {
            appSettingReader = new ClaimsTagsReader(typeof(ClaimsTagsList).Namespace, "appsettings.json");
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