namespace WeeControl.SharedKernel;

public static class Api
{
    public static class Essential
    {
        private const string Route = nameof(Api) + "/" + nameof(Essential) + "/";
        
        public static class Authorization
        {
            /// <summary>
            /// Post: Submit 
            /// </summary>
            public const string EndPoint = Route + nameof(Authorization);
        }
        
        public static class User
        {
            public const string EndPoint = Route + nameof(User);
            
            public static class Session
            {
                public const string EndPoint = Route + nameof(User) + "/" + nameof(Session);
            }
            
            public const string ResetPassword = Route + nameof(User) + "/Reset";
        }
        
        public static class Admin
        {
            public const string EndPoint = Route + nameof(Admin);
            public const string User = Route + nameof(Admin) + "/User";
            //public const string Territory = Route + nameof(Admin) + "/Territory";
        }

        public static class Territory
        {
            public const string EndPoint = Route + nameof(Territory);
        }

        
    }
}