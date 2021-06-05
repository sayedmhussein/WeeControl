using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MySystem.Web.Api.Service;

namespace MySystem.MySystem.Api.Service
{
    public class AuthenticationService
    {
        public static void ConfigureAuthorizationService(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }

        public static void ConfigureJwtService(JwtBearerOptions options)
        {
            //options.TokenValidationParameters = new JwtService(Configuration["Jwt:Key"]).ValidationParameters;

            //options.Events = new JwtBearerEvents
            //{
            //    OnMessageReceived = context =>
            //    {
            //        try
            //        {
            //            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            //            context.Token = token;
            //        }
            //        catch { }

            //        return Task.CompletedTask;
            //    },
            //};
        }
    }
}
