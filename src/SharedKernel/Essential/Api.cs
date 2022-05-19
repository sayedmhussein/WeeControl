namespace WeeControl.SharedKernel.Essential;

public static class Api
{
    public static class Essential
    {
        private const string Route = nameof(Api) + "/" + nameof(Essential) + "/";
        
        public static class Users
        {
            public const string Login = Route + nameof(Users) + "/Login";
            public const string Logout = Route + nameof(Users) + "/Login";
            public const string Bla = Route + nameof(Users) + "/Login{id}";
        }
        
        public static class Admin
        {
            public const string User = Route + nameof(Admin) + "/User";
            public const string Territory = Route + nameof(Admin) + "/Territory";
        }
    }
}