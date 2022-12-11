using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeeControl.ApiApp.Application;
using WeeControl.ApiApp.Infrastructure;
using WeeControl.ApiApp.Persistence;
using WeeControl.ApiApp.WebApi.Middlewares;
using WeeControl.ApiApp.WebApi.Services;
using WeeControl.ApiApp.WebApi.Services.Security;

namespace WeeControl.ApiApp.WebApi;

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

        services.AddHttpContextAccessor();

        services.AddUserSecurityService();
        services.AddApplication();
        services.AddInfrastructure(Configuration);

        _ = Configuration["UseInMemoryDb"] == false.ToString() ? 
            services.AddPersistenceAsPostgres(Configuration, Assembly.GetExecutingAssembly().GetName().Name) :
            services.AddPersistenceAsInMemory();


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