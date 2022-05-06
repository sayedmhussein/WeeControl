using System.Net;

namespace WeeControl.Frontend.ServiceLibrary.BoundedContexts.Authorization
{
    public class AuthorizationLink
    {
        public const string Route = "Api/Credentials/";

        public class Register
        {
            public const string EndPoint = "Register";
            public static readonly HttpMethod Method = HttpMethod.Post;
            public const string Relative = Route + EndPoint;
            public static string Absolute(string server) => server + Relative;
            public const string Version = "1.0";

        }

        public class Login
        {
            public const string EndPoint = "";
            public static readonly HttpMethod Method = HttpMethod.Post;
            public const string Relative = Route + EndPoint;
            public static string Absolute(string server) => server + Relative;
            public const string Version = "1.0";
            public readonly HttpStatusCode[] ResponseCodes =
            {
                HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.NotFound
            };
        }

        public static class RequestRefreshToken
        {
            public const string EndPoint = "";
            public static readonly HttpMethod Method = HttpMethod.Put;
            public const string Relative = Route + EndPoint;
            public static string Absolute(string server) => server + Relative;
            public const string Version = "1.0";
        }

        public static class Logout
        {
            public const string EndPoint = "";
            public static readonly HttpMethod Method = HttpMethod.Delete;
            public const string Relative = Route + EndPoint;
            public static string Absolute(string server) => server + Relative;
            public const string Version = "1.0";
        }
    }
}
