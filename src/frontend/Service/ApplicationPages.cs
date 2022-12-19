using WeeControl.Common.SharedKernel;

namespace WeeControl.Frontend.AppService;

public static class ApplicationPages
{
    public const string SplashPage = "Splash";
    public const string AuthenticationPage = "Authentication";
    public const string HomePage = "Index";

    
    
    

    
    
    public static class Essential
    {
        
        public const string HomePage = "Index";
        public const string UserPage = "User";
        public const string OtpPage = "Otp";
    }

    public static class Elevator
    {
        public const string FieldPage = nameof(ClaimsValues.ClaimTypes.Field);
        public const string AdminPage = nameof(ClaimsValues.ClaimTypes.Administrator);
        public const string SalesPage = nameof(ClaimsValues.ClaimTypes.Sales);
    }
}