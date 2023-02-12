﻿namespace WeeControl.Host.WebApiService;

public class ControllerApi
{
    public static class Authorization
    {
        public const string Route = "Api/Authorization";
    }
    
    public static class User
    {
        public const string AuthorizationRoute = "Api/User";
        
        public const string RegisterEndpoint = "Register";
        public const string PasswordEndpoint = "Password";
        public const string HomeEndpoint = "Register";
    }
}