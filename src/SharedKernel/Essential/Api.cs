namespace WeeControl.SharedKernel.Essential;

public static class Api
{
    public static class Essential
    {
        private const string Route = nameof(Api) + "/" + nameof(Essential) + "/";
        
        public static class User
        {
            public const string Base = Route + nameof(User);
            public const string Session = Route + nameof(Essential.User) + "/Session";
        }
        
        public static class Admin
        {
            public const string Base = Route + nameof(Admin);
            public const string User = Route + nameof(Admin) + "/User";
            public const string Territory = Route + nameof(Admin) + "/Territory";
        }
    }
}