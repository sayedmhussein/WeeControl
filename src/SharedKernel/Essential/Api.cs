namespace WeeControl.SharedKernel.Essential;

public static class Api
{
    public static class Essential
    {
        private const string Route = nameof(Api) + "/" + nameof(Essential) + "/";
        
        public static class Users
        {
            public const string Login = nameof(Api) + "/" + nameof(Essential) + "/" + nameof(Users) + "/Login";
            public const string Logout = nameof(Essential) + "/" + nameof(Users) + "/Login";
            public const string Bla = nameof(Essential) + "/" + nameof(Users) + "/Login{id}";
        }
        
        public static class Admin
        {
            public const string User = Route + "User";
            public const string Territory = Route + "Territory";
        }
    }
}