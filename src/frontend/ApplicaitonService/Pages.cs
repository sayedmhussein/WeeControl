using WeeControl.SharedKernel;

namespace WeeControl.Frontend.ApplicationService;

public static class Pages
{
    public static class Shared
    {
        public const string SplashPage = "Splash";
        public const string IndexPage = "Index";
        public const string MenuPage = "Menu";
        public const string InvalidPage = "Invalid";
        public const string NoInternetPage = "NoInternet";
        public const string NotFoundPage = "Notfound";
        public const string ErrorPage = "Error";
    }
    
    public static class Essential
    {
        public static class Home
        {
            public const string SplashPage = "/";
            public const string HomePage = "Home";
        }
        
        public static class Authentication
        {
            public const string LoginPage = "Login";
            public const string LogoutPage = "Logout";
        }
        
        public static class User
        {
            public const string RegisterPage = "Register";
            public const string SetNewPasswordPage = "PasswordReset";
            public const string ForgotMyPasswordPage = "ForgotPassword";
        }
        
        public static class Admin
        {
            public const string Page = "Administrator";
        }
    }
    
    public static class Elevator
    {
        public static class Field
        {
            public const string FieldPage = nameof(ClaimsValues.ClaimTypes.Field);
        }
    }
}