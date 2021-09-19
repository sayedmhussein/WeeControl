using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WeeControl.Backend.WebApi.StartupOptions
{
    public class AuthenOptions
    {
        public static void ConfigureAuthorizationService(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
