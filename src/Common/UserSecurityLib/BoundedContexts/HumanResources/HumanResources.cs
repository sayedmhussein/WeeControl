using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WeeControl.Common.UserSecurityLib.BoundedContexts.HumanResources;

public static class HumanResourcesData
{
    public const string Role = "hr_r";

    public static Dictionary<string, string> GetTagDictionary()
    {
        var fieldInfos =
            typeof(Claims.Tags).GetFields(BindingFlags.Static | BindingFlags.Public);
        return fieldInfos.ToDictionary(info => info.Name, info => info?.GetValue(null)?.ToString());
    }

    public static class Policies
    {
        public const string CanAlterTerritories = "CanAlterTerritories";
            
        public const string CanAlterEmployee = "CanAddNewEmployee";
        public const string CanEditEmployeeDetails = "CanEditEmployeeDetails";
    }
        
    public static class Claims
    {
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