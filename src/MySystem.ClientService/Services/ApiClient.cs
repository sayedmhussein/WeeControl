using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using MySystem.ClientService.Interfaces;

namespace MySystem.ClientService.Services
{
    public static class ApiClient
    {
        public static string GetUri(Route route)
        {
            return route switch
            {
                Route.Authentication_Login => "/Api/Authentication/Login",
                Route.Authentication_Token => "/Api/Authentication/Token",
                Route.Authentication_Logout => "/Api/Authentication/Logout",
                _ => null,
            };
        }

        public enum Route
        {
            Authentication_Login, Authentication_Token, Authentication_Logout
        };
    }
}
