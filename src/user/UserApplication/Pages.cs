namespace WeeControl.User.UserApplication;

public static class Pages
{
    public static class Authentication
    {
        public const string LoginPage = "Login";
        public const string Logout = "Logout";
    }

    public static class User
    {
        public const string Register = "Register";
        public const string ResetPassword = "PasswordReset";
        public const string RequestNewPassword = "ForgotPassword";
    }

    public static class Home
    {
        public const string Splash = "Splash";
        public const string Index = "Index";
        public const string Menu = "Menu";
        public const string Invalid = "Invalid";
        public const string NoInternet = "NoInternet";
    }
}