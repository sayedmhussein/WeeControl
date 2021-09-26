using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WeeControl.Common.UserSecurityLib
{
    public static class SecurityClaims
    {
        public static class HumanResources
        {
            public static Dictionary<string, string> GetClaimTypes()
            {
                var humanResourcesFieldInfos =
                    typeof(HumanResources).GetFields(BindingFlags.Static | BindingFlags.Public);

                return humanResourcesFieldInfos.ToDictionary(info => info.Name, info => info.GetValue(null).ToString());
            }
            
            public static Dictionary<string, string> GetClaimTags()
            {
                var humanResourcesFieldInfos =
                    typeof(Tags).GetFields(BindingFlags.Static | BindingFlags.Public);

                return humanResourcesFieldInfos.ToDictionary(info => info.Name, info => info.GetValue(null).ToString());
            }

            public const string Role = "hr_r";
            public const string Session = "hr_s";
            public const string Territory = "hr_t";
            
            public static class Tags
            {
                public const string SuperUser = "sudo";
                public const string Manager = "mgr";
                public const string DataEntry = "dtrs";
            }
        }
    }
}