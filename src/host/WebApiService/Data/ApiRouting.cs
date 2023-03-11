namespace WeeControl.Host.WebApiService.Data;

public static class ApiRouting
{
    public static class Essentials
    {
        public static class Session
        {
            public const string Route = "Api/Authorization";
        }

        public static class User
        {
            public const string Route = "Api/User";

            public const string RegisterEndpoint = "Register";
            public const string PasswordEndpoint = "Password";
            public const string HomeEndpoint = "Register";
            public const string NotificationEndpoint = "Notification";
        }
    }
}