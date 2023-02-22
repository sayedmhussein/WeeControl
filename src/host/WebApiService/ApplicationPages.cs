using WeeControl.Core.SharedKernel;

namespace WeeControl.Host.WebApiService;

public static class ApplicationPages
{
    public static class Essential
    {
        public const string LoginPage = "Authentication";
        public const string OtpPage = "AuthenticationOtp";
        
        public const string HomePage = "Index";
    }
    
    public static class Elevator
    {
        public const string FieldPage = nameof(ClaimsValues.ClaimTypes.Field);
        public const string AdminPage = nameof(ClaimsValues.ClaimTypes.Administrator);
        public const string SalesPage = nameof(ClaimsValues.ClaimTypes.Sales);
    }
}