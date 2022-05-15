using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WeeControl.WebApi.StartupOptions;

public class AuthenOptions
{
    public static void ConfigureAuthorizationService(AuthenticationOptions options)
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    }
}