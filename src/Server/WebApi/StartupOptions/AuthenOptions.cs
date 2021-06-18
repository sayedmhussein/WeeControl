using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using WeeControl.Server.WebApi.Services;

namespace WeeControl.Server.WebApi.StartupOptions
{
    public class AuthenOptions
    {
        public static void ConfigureAuthorizationService(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }

        public static void ConfigureJwtService(JwtBearerOptions options, IConfiguration configuration)
        {
            options.TokenValidationParameters = new JwtService(configuration["Jwt:Key"]).ValidationParameters;

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    try
                    {
                        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                        context.Token = token;
                    }
                    catch { }

                    return Task.CompletedTask;
                },
            };
        }
    }
}
