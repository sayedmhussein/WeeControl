namespace WeeControl.User.UserServiceCore;

public static class Pages
{
    public static class Authentication
    {
        public const string Login = "LoginPage";
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
        public const string Invalid = "Menu";
        public const string NoInternet = "NoInternet";
    }
}