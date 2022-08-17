

namespace WeeControl.SharedKernel;

public static class Api
{
    public static class Essential
    {
        private const string RouteBase = nameof(Api) + "/" + nameof(Essential) + "/";
        
        public static class Authorization
        {
            public const string Route = RouteBase + nameof(Authorization);
        }
        
        public static class User
        {
            public static class ServerEndPoints
            {
                public const string Notification = nameof(Notification);
                public const string Password = nameof(Password);
            }
        }
        
        public static class Customer
        {
            public const string CustomerRoute = RouteBase + nameof(Customer);
            
            public static class EndPoints
            {
                public static class Server
                {
                    public const string Notification = User.ServerEndPoints.Notification;
                    public const string Password = User.ServerEndPoints.Password;
                }
                
                public static class Service
                {
                    public const string Customer = CustomerRoute;
                    public const string Notification = CustomerRoute + "/" + Server.Notification;
                    public const string Password = CustomerRoute + "/" + Server.Password;
                }
            }
        }
        
        public static class Employee
        {
            public const string EmployeeRoute = RouteBase + nameof(Employee);
            
            public static class EndPoints
            {
                public static class Server
                {
                    public const string Notification = User.ServerEndPoints.Notification;
                    public const string Password = User.ServerEndPoints.Password;
                }
                
                public static class Service
                {
                    public const string Notification = EmployeeRoute + "/" + Server.Notification;
                    public const string Password = EmployeeRoute + "/" + Server.Password;
                }
            }
        }
        
        public static class Routes
        {
            public const string Territory = RouteBase + nameof(Territory);
        }
    }
}