using System.Linq;
using System.Threading.Tasks;
using Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySystem.Api.Service;
using MySystem.Application.Common.Interfaces;
using MySystem.Infrastructure;
using MySystem.MySystem.Api.Middleware;
using MySystem.MySystem.Api.Service;
using MySystem.Persistence;
using MySystem.SharedKernel.Interfaces;
using MySystem.SharedKernel.Services;
using MySystem.Web.Api.Security.TokenRefreshment;
using MySystem.Web.Api.Service;

namespace MySystem.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddHttpContextAccessor();

            services.AddApplication();
            services.AddInfrastructure(Configuration);
            services.AddPersistence(Configuration);

            services.AddApiVersioning(ApiVersionService.ConfigureApiVersioning);
            services.AddSwaggerGen(SwaggerService.ConfigureSwaggerGen);

            services.AddSingleton<ISharedValues, SharedValues>();

            services.AddScoped<ICurrentUserInfo, UserInfoService>();
            services.AddSingleton<IJwtService>(provider => new JwtService(Configuration["Jwt:Key"]));

            AuthenticationConfig(services);


            //AuthorizationConfig(services);
            services.AddSingleton<IAuthorizationHandler, TokenRefreshmentHandler>();
            services.AddAuthorization(AuthorizationService.ConfigureAuthorizationOptions);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(SwaggerService.ConfigureSwaggerUI);
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

        private void AuthenticationConfig(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
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
    }
}