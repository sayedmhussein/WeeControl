using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WeeControl.ApiApp.WebApi.StartupOptions;

public class AuthenOptions
{
    public static void ConfigureAuthorizationService(AuthenticationOptions options)
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    }
}