using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeeControl.Host.WebApi.Middlewares;
using WeeControl.Host.WebApi.Services;
using WeeControl.Host.WebApi.Services.Security;

namespace WeeControl.Host.WebApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        // services.AddControllers(
        //     options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

        services.AddHttpContextAccessor();

        services.AddUserSecurityService();

        services.AddDomainDrivenDesignService(Configuration);

        services.AddApiVersionService();
        services.AddUserInfo();

        services.AddCoresService();

        services.AddAuthenticationService(Configuration["Jwt:Key"]);
        services.AddSwaggerService();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(SwaggerServices.ConfigureSwaggerUi);
        }
        else
        {
            app.UseHttpsRedirection();
        }

        app.UseCustomExceptionHandler();

        app.UseRouting();

        app.UseCors("AllowAny");

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}