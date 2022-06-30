using System.Reflection;

namespace WeeControl.SharedKernel;

public static class ClaimsTagsList
{
    public static Dictionary<string, string> GetClaimsDictionary()
    {
        var fieldInfos =
            typeof(Claims).GetFields(BindingFlags.Static | BindingFlags.Public);
        return fieldInfos.ToDictionary(info => info.Name, info => info?.GetValue(null)?.ToString())!;
    }
    
    public static Dictionary<string, string> GetTagsDictionary()
    {
        var fieldInfos =
            typeof(Tags).GetFields(BindingFlags.Static | BindingFlags.Public);
        return fieldInfos.ToDictionary(info => info.Name, info => info?.GetValue(null)?.ToString())!;
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
        public const string Manager = "e_man";
        public const string Depute = "e_dep";
        public const string Officer = "e_off";
        public const string Senior = "e_sen";
        public const string Supervisor = "e_sup";
        public const string Auditor = "e_aud";
        public const string Executive = "e_exe";
        public const string Trainee = "e_tra";
    }
}