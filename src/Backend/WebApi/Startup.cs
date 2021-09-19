using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeeControl.Backend.Application;
using WeeControl.Backend.Application.Common.Interfaces;
using WeeControl.Backend.Infrastructure;
using WeeControl.Backend.Persistence;
using WeeControl.Backend.WebApi.Middlewares;
using WeeControl.Backend.WebApi.Services;
using WeeControl.Backend.WebApi.StartupOptions;
using WeeControl.UserSecurityLib;
using WeeControl.UserSecurityLib.Interfaces;
using WeeControl.UserSecurityLib.Services;

namespace WeeControl.Backend.WebApi
{
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

            services.AddApiAuthorizationService();
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            services.AddPersistenceAsPostgreSql(Configuration, Assembly.GetExecutingAssembly().GetName().Name);

            services.AddApiVersioning(ApiVersionOptions.ConfigureApiVersioning);
            services.AddSwaggerGen(SwaggerOptions.ConfigureSwaggerGen);

            services.AddScoped<ICurrentUserInfo, UserInfoService>();
            services.AddSingleton<IJwtService, JwtService>();

            services.AddCors(c => c.AddPolicy("AllowAny", builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
            }));

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
            
            app.UseCors("AllowAny");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}