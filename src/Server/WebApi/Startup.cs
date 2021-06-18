using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeeControl.Server.Application;
using WeeControl.Server.Application.Common.Interfaces;
using WeeControl.Server.Infrastructure;
using WeeControl.Server.Persistence;
using WeeControl.Server.WebApi.Middlewares;
using WeeControl.Server.WebApi.Security.TokenRefreshment.CustomHandlers;
using WeeControl.Server.WebApi.Services;
using WeeControl.Server.WebApi.StartupOptions;

namespace WeeControl.Server.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddHttpContextAccessor();

            services.AddApplication();
            services.AddInfrastructure(Configuration);
            services.AddPersistenceAsPostgreSQL(Configuration, Assembly.GetExecutingAssembly().GetName().Name);

            services.AddApiVersioning(ApiVersionOptions.ConfigureApiVersioning);
            services.AddSwaggerGen(SwaggerOptions.ConfigureSwaggerGen);

            services.AddScoped<ICurrentUserInfo, UserInfoService>();
            services.AddSingleton<IJwtService>(provider => new JwtService(Configuration["Jwt:Key"]));


            services.AddAuthentication(AuthenOptions.ConfigureAuthorizationService).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new JwtService(Configuration["Jwt:Key"]).ValidationParameters;

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
            });

            services.AddSingleton<IAuthorizationHandler, TokenRefreshmentHandler>();
            services.AddAuthorization(AuthorOptions.ConfigureAuthOptions);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(SwaggerOptions.ConfigureSwaggerUI);
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseCustomExceptionHandler();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}