using WeeControl.SharedKernel;

namespace WeeControl.Frontend.ApplicationService;

public static class Pages
{
    public static class Anonymous
    {
        public const string RootPage = "/";
        public const string SplashPage = "Splash";
        public const string IndexPage = "Index";
        public const string CustomerRegisterPage = "Register";
        public const string LoginPage = "Login";
    }
    
    public static class Common
    {
        public const string PasswordChangePage = "PasswordChange";
    }

    public static class Customer
    {
        
    }
    
    public static class Elevator
    {
        public const string FieldPage = nameof(ClaimsValues.ClaimTypes.Field);
        public const string AdminPage = nameof(ClaimsValues.ClaimTypes.Administrator);
        public const string SalesPage = nameof(ClaimsValues.ClaimTypes.Sales);
    }
}