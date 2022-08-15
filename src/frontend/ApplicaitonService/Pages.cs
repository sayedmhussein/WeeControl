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

    public static class Customer
    {
        
    }
    
    public static class Elevator
    {
        public static class Field
        {
            public const string FieldPage = nameof(ClaimsValues.ClaimTypes.Field);
        }
    }
}