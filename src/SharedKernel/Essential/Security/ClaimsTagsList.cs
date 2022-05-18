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

    public static class Claims
    {
        public const string Session = "e_s";
        public const string Territory = "e_r";
        
        public const string Developer = "e_dv";
        public const string Administrator = "e_ad";
        public const string HumanResource = "e_hr";
        public const string Director = "e_dr";
        public const string Finance = "e_fn";
        public const string Logistics = "e_lg";
        public const string Sales = "e_sl";
        public const string Field = "e_fd";
    }
    
    public static class Tags
    {
        public const string SuperUser = "e_su";
        public const string Manager = "e_mgr";
        public const string Depute = "e_dat";
        public const string Officer = "e_dat";
        public const string Senior = "e_dat";
        public const string Supervisor = "e_dat";
        public const string Auditor = "e_dat";
        public const string Executive = "e_dat";
        public const string Trainee = "e_dat";
    }
}