using System;
using System.Net.Http;

namespace WeeControl.Common.BoundedContext.Credentials.Operations
{
    public class ApiRouteLink
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
