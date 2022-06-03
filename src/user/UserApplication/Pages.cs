namespace WeeControl.User.UserApplication;

public static class Pages
{
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

    public static class Home
    {
        public const string SplashPage = "Splash";
        public const string IndexPage = "Index";
        public const string Menu = "Menu";
        public const string Invalid = "Invalid";
        public const string NoInternet = "NoInternet";
    }
    
    public static class Admin
    {
        public const string Page = "Administrator";
    }
}