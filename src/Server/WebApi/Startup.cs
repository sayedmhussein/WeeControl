using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySystem.Api.Services;
using MySystem.Application.Common.Interfaces;
using MySystem.Infrastructure;
using MySystem.MySystem.Api.Middlewares;
using MySystem.MySystem.Api.Service;
using MySystem.Persistence;
using MySystem.SharedKernel.Interfaces.Values;
using MySystem.SharedKernel.Services;
using MySystem.Web.Api.Security.TokenRefreshment.CustomHandlers;
using MySystem.Web.Api.Services;
using MySystem.Web.Api.StartupOptions;

namespace MySystem.Web.Api
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

            services.AddSingleton<ICommonValues, CommonValues>();
            services.AddSingleton<ITerritoryValues, TerritoryValues>();
            services.AddSingleton<IEmployeeValues, EmployeeValues>();

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