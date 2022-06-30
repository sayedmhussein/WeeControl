namespace WeeControl.SharedKernel;

public static class Api
{
    public static class Essential
    {
        private const string RouteBase = nameof(Api) + "/" + nameof(Essential) + "/";
        
        public static class Authorization
        {
            /// <summary>
            /// Post: Get token using username and password.
            /// Put: Get token using token.
            /// Delete: Terminate existing session. 
            /// </summary>
            public const string Route = RouteBase + nameof(Authorization);
        }
        
        public static class User
        {
            /// <summary>
            /// Get: Get list of users.
            /// Get(username): Get user details.
            /// Post: Add new user.
            /// Put: Update existing user.
            /// Delete: Remove existing user.
            /// </summary>
            public const string Route = RouteBase + nameof(User);

            /// <summary>
            /// Get: Get existing session(s) of current user.
            /// Get(username): Get existing session(s) of specific user.
            /// Delete(id): Terminate existing session. 
            /// </summary>
            public const string Session = nameof(Session);

            /// <summary>
            /// Post: Send email and username to get temporary password.
            /// Patch: Update password in database.
            /// </summary>
            public const string ResetPassword = "Reset";

            public const string ResetPasswordEndPoint = Route + "/" + ResetPassword;
        }
        
        public static class Admin
        {
            public const string Route = RouteBase + nameof(Admin);
        }

        public static class Territory
        {
            /// <summary>
            /// Get: Get list of territories.
            /// Put: Add or update existing territory.
            /// </summary>
            public const string EndPoint = RouteBase + nameof(Territory);
        }

        
    }
}