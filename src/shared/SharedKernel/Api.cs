

namespace WeeControl.SharedKernel;

public static class Api
{
    public static class Essential
    {
        private const string RouteBase = nameof(Api) + "/" + nameof(Essential) + "/";
        
        public static class Routes
        {
            public const string Territory = RouteBase + nameof(Territory);
            public const string Authorization = RouteBase + nameof(Authorization);

            public const string Customer = RouteBase + nameof(Customer);
            public const string Employee = RouteBase + nameof(Employee);
        }

        public static class EndPoints
        {
            public const string Notification = nameof(Notification);
            public const string Password = nameof(Password);
        }
    }
}