using System.Reflection;

namespace WeeControl.SharedKernel.Essential.Security;

public static class ClaimsTagsList
{
    public static Dictionary<string, string> GetClaimsDictionary()
    {
        var fieldInfos =
            typeof(Claims).GetFields(BindingFlags.Static | BindingFlags.Public);
        return fieldInfos.ToDictionary(info => info.Name, info => info?.GetValue(null)?.ToString());
    }
    
    public static Dictionary<string, string> GetTagsDictionary()
    {
        var fieldInfos =
            typeof(Tags).GetFields(BindingFlags.Static | BindingFlags.Public);
        return fieldInfos.ToDictionary(info => info.Name, info => info?.GetValue(null)?.ToString());
    }

    [Obsolete("To put policy names inside DTO class")]
    public static class Policies
    {
        public const string CanAlterTerritories = "CanAlterTerritories";
        public const string CanAlterEmployee = "CanAddNewEmployee";
    }
        
    public static class Claims
    {
        public const string SessionClaim = "e_s";
        public const string TerritoryClaim = "e_r";
        
        public const string DeveloperClaim = "e_dv";
    }
    
    public static class Tags
    {
        public const string SuperUser = "e_su";
        public const string Manager = "e_mgr";
        public const string DataEntry = "e_dat";
    }
}