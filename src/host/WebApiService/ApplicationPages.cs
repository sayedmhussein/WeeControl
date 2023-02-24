using System.Reflection;
using WeeControl.Core.SharedKernel;

namespace WeeControl.Host.WebApiService;

public static class ApplicationPages
{
    public static class Essential
    {
        public const string LoginPage = "Authentication";
        public const string OtpPage = "AuthenticationOtp";
        
        public const string HomePage = "Index";
        public const string UserPage = "User";
    }
    
    public static class Elevator
    {
        public static Dictionary<string, string> GetListOfPages()
        {
            var fieldInfos =
                typeof(Elevator).GetFields(BindingFlags.Static | BindingFlags.Public);
            return fieldInfos.ToDictionary(info => info.Name, info => info?.GetValue(null)?.ToString())!;
        }
        
        public const string FieldPage = nameof(ClaimsValues.ClaimTypes.Field);
        public const string AdminPage = nameof(ClaimsValues.ClaimTypes.Administrator);
        public const string SalesPage = nameof(ClaimsValues.ClaimTypes.Sales);
    }
}